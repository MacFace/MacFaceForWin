/*
 * AlphaForm.cs
 * $Id: AlphaForm.cs,v 1.3 2003/07/10 20:07:16 Mayuki Sawatari Exp $
 *
 * Copyright (c) 2003 Mayuki Sawatari, All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions
 * are met:
 * 1. Redistributions of source code must retain the above copyright
 *    notice, this list of conditions and the following disclaimer.
 * 2. Redistributions in binary form must reproduce the above copyright
 *    notice, this list of conditions and the following disclaimer in the
 *    documentation and/or other materials provided with the distribution.
 *
 * THIS SOFTWARE IS PROVIDED BY THE MISUZILLA.ORG ``AS IS'' AND
 * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
 * ARE DISCLAIMED.  IN NO EVENT SHALL THE AUTHOR OR CONTRIBUTORS BE LIABLE
 * FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
 * DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS
 * OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
 * HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
 * LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY
 * OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF
 * SUCH DAMAGE.
 */

using System;
using System.Timers;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Misuzilla.Windows.Forms
{
	/// <summary>ピクセル単位アルファブレンドのフォーム</summary>
	/// <remarks>
	/// 	<para>
	/// 		Windows2000/XPのレイヤードウィンドウを利用し
	/// 		ピクセル単位で背後のウィンドウと半透明合成できるフォーム。
	/// 	</para>
	/// 	<para>
	/// 		このフォームは通常のフォームと違い<see cref="Update" />メソッドで描画更新を指示する必要があります。
	/// 		<see cref="System.Windows.Forms.Form.Paint" />イベントなど再描画のイベントも発生しません。
	/// 	</para>
	/// </remarks>
	/// <seealso cref="System.Windows.Forms.Form" />
	public class AlphaForm : System.Windows.Forms.Form
	{
		private Byte    m_opacity   = 255;
		private SByte   m_fadeRatio = 25;
		private Boolean m_movedrag  = false;
		private Boolean m_transMouseMsg = false;

		private System.Drawing.Bitmap   m_alphaMap = null;
		private System.Drawing.Graphics m_graphics = null;

		private IntPtr m_hDC    = IntPtr.Zero;
		private IntPtr m_hMemDC = IntPtr.Zero;
		
		private Win32.Point m_pForm;
		private Win32.Point m_pSurface;
		private Win32.Size  m_sBitmap;

#if FADE_USE_TIMER
		private System.Timers.Timer m_fadeTimer = null;
#endif

		public
		AlphaForm()
		{
			if (OSFeature.Feature.GetVersionPresent(OSFeature.LayeredWindows) == null)
				throw new ApplicationException("Layered Windows is not supported");
			
			m_alphaMap = new Bitmap(100, 100);
			m_graphics = Graphics.FromImage(m_alphaMap);
			this.PrepareDC();
		}

		/// <summary>フォームに関連付けられた<see cref="System.Drawing.Graphics"/>オブジェクトを取得します。</summary>
		/// <remarks>
		/// 	描画にはこの<see cref="System.Drawing.Graphics"/>オブジェクトを利用する必要があります。
		/// 	また、描画した後<see cref="Misuzilla.Windows.Forms.AlphaForm.Update"/>メソッドを呼ぶことで表示が更新されます。
		/// </remarks>
		/// <see cref="System.Drawing.Graphics"/>
		public System.Drawing.Graphics
		Graphics
		{
			get
			{
				return m_graphics;
			}
		}
		
		protected override void
		Dispose(bool disposing)
		{
			this.CloseDC();
			base.Dispose(disposing);
		}

		protected override CreateParams
		CreateParams
		{
			get
			{
				CreateParams cp = base.CreateParams;
				cp.ExStyle |= Win32.WS_EX_LAYERED; // WS_EX_LAYERED
				return cp;
			}
		}
		
		protected override void
		OnResize(EventArgs e)
		{
			base.OnResize(e);
			if (m_graphics != null) m_graphics.Dispose();
			if (m_alphaMap != null) m_alphaMap.Dispose();
			m_alphaMap = new Bitmap(this.Width, this.Height);
			m_graphics = Graphics.FromImage(m_alphaMap);
			this.PrepareDC();
			this.SetSize();
			this.SetPosition();
		}
		protected override void
		OnMove(EventArgs e)
		{
			base.OnMove(e);
			this.SetPosition();
		}
		
		
		private void
		SetSize()
		{
			lock (this) {
				m_sBitmap.cx = m_alphaMap.Width;
				m_sBitmap.cy = m_alphaMap.Height;
			}
		}
		private void
		SetPosition()
		{
			m_pForm.x = this.Left;
			m_pForm.y = this.Top;
			m_pSurface.x = 0;
			m_pSurface.y = 0;
		}
		
		private void
		PrepareDC()
		{
			this.CloseDC();
			m_hDC = Win32.GetDC(IntPtr.Zero);
			m_hMemDC = Win32.CreateCompatibleDC(m_hDC);
			if ((m_hDC == IntPtr.Zero) || (m_hMemDC == IntPtr.Zero)) {
				this.CloseDC();
				throw new ApplicationException("Couldn't allocate DeviceContext");
			}
		}
		private void
		CloseDC()
		{
			if (m_hMemDC != IntPtr.Zero) Win32.DeleteDC(m_hMemDC);
			if (m_hDC != IntPtr.Zero) Win32.ReleaseDC(IntPtr.Zero, m_hDC);
		}
		
		/// <summary>フォームの表示を更新します。</summary>
		/// <remarks>
		/// 	このメソッドを呼ぶことで表示が更新されます。
		/// </remarks>
		public override void
		Refresh()
		{
			Update(m_opacity);
		}
		/// <summary>フォームの表示を更新します。</summary>
		/// <remarks>
		/// 	このメソッドを呼ぶことで表示が更新されます。
		/// </remarks>
		public new void
		Update()
		{
			Update(m_opacity);
		}
		
		/// <summary>フォームの表示を更新します。</summary>
		/// <remarks>
		/// 	このメソッドを呼ぶことで表示が更新されます。
		/// </remarks>
		/// <param name="bOpacity">更新する際のフォームの透明度</param>
		[MethodImpl(MethodImplOptions.Synchronized)]
		public void
		Update(Byte bOpacity)
		{
			m_opacity = bOpacity;

			if (m_alphaMap == null) return;
			if (m_alphaMap.PixelFormat != PixelFormat.Format32bppArgb) return;

			Win32.BlendFunction bf;
			bf.BlendOp = 0; // AC_SRC_OVER
			bf.BlendFlags = 0;
			bf.AlphaFormat = 1; // AC_SRC_ALPHA
			bf.SourceConstantAlpha = bOpacity;
			
			m_opacity = bOpacity;
			
			IntPtr hBitmapOld = IntPtr.Zero;
			IntPtr hBitmap = IntPtr.Zero;
			
			//lock (this) {
			//	lock (m_graphics) {
					hBitmap = m_alphaMap.GetHbitmap(Color.FromArgb(0));
					hBitmapOld = Win32.SelectObject(m_hMemDC, hBitmap);

					if (Win32.UpdateLayeredWindow(
							this.Handle, 
							m_hDC, 
							ref m_pForm, 
							ref m_sBitmap,
							m_hMemDC, 
							ref m_pSurface, 
							0, 
							ref bf, 
							0x2) == 0
					){
						//throw new ApplicationException("UpdateLayerdWindow was failed");
					}
					if (hBitmap != IntPtr.Zero) {
						Win32.SelectObject(m_hMemDC, hBitmapOld);
						Win32.DeleteObject(hBitmap);
					}
			//	}
			//}
		}
		
		public new Single
		Opacity
		{
			get
			{
				return (m_opacity / 255.0F);
			}
			set
			{
				if (value > 1.0F)
					value = 1.0F;
				else if (value < 0.0F)
					value = 0.0F;
				m_opacity = (Byte)(value * 255.0F);
				Update(m_opacity);
			}
		}

		/// <summary>マウスドラッグによってフォームの移動をするかどうかを指定します。</summary>
		/// <remarks>
		/// 	このプロパティが指定されている場合フォーム上で右ドラッグを行うと
		/// 	フォームの移動が行えます。
		/// </remarks>
		public Boolean
		MoveAtFormDrag
		{
			get
			{
				return m_movedrag;
			}
			set
			{
				m_movedrag = value;
			}
		}

		/// <summary>マウスメッセージがフォームを透過するかどうかを指定します。</summary>
		/// <remarks>
		/// 	このプロパティが指定されている場合マウスメッセージは
		/// 	フォームを透過し、背後のフォームに伝わります。
		/// </remarks>
		public Boolean
		TransparentMouseMessage
		{
			get
			{
				return m_transMouseMsg;
			}
			set
			{
				Int32 exStyle = Win32.GetWindowLong(this.Handle, Win32.GWL_EXSTYLE);

				if (value)
					exStyle |= Win32.WS_EX_TRANSPARENT;
				else if ((exStyle & Win32.WS_EX_TRANSPARENT) == Win32.WS_EX_TRANSPARENT)
					exStyle ^= Win32.WS_EX_TRANSPARENT;

				Win32.SetWindowLong(this.Handle, Win32.GWL_EXSTYLE, exStyle);
				m_transMouseMsg = value;
			}
		}
		
		protected override void
		OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (m_movedrag) {
				if (e.Button == MouseButtons.Left) {
					Win32.ReleaseCapture();
					// Int32 WM_NCLBUTTONDOWN = 0xa1;
					// Int32 HTCAPTION = 0x02;
					Win32.SendMessage(this.Handle, 0xa1, 0x02, IntPtr.Zero);
				}
			}
		}
		
		/// <summary>フォームをフェードインします。</summary>
		/// <remarks>
		/// 	フォームの透明度を指定された時間ごとに上げていき表示します。
		/// </remarks>
		/// <param name="transMilliSec">透明度の更新間隔</param>
		public void
		FadeinWindow(Double transMilliSec)
		{
			FadeWindow(transMilliSec, (SByte)(25), 255, 0);
		}
		/// <param name="transMilliSec">透明度の更新間隔</param>
		/// <param name="fadeRatio">透明度の変化率</param>
		public void
		FadeinWindow(Double transMilliSec, Byte fadeRatio)
		{
			FadeWindow(transMilliSec, (SByte)(fadeRatio), 255, 0);
		}
		/// <param name="transMilliSec">透明度の更新間隔</param>
		/// <param name="fadeRatio">透明度の変化率</param>
		/// <param name="opacityMax">透明度の最大値</param>
		public void
		FadeinWindow(Double transMilliSec, Byte fadeRatio, Byte opacityMax)
		{
			FadeWindow(transMilliSec, (SByte)(fadeRatio), opacityMax, 0);
		}
		/// <summary>フォームをフェードアウトします。</summary>
		/// <remarks>
		/// 	フォームの透明度を指定された時間ごとに下げていき完全に透明にします。
		/// </remarks>
		/// <param name="transMilliSec">透明度の更新間隔</param>
		public void
		FadeoutWindow(Double transMilliSec)
		{
			FadeWindow(transMilliSec, (SByte)(-25), 255, 0);
		}
		/// <param name="transMilliSec">透明度の更新間隔</param>
		/// <param name="fadeRatio">透明度の変化率</param>
		public void
		FadeoutWindow(Double transMilliSec, Byte fadeRatio)
		{
			FadeWindow(transMilliSec, (SByte)(-fadeRatio), 255, 0);
		}
		/// <param name="transMilliSec">透明度の更新間隔</param>
		/// <param name="fadeRatio">透明度の変化率</param>
		/// <param name="opacityMin">透明度の最小値</param>
		public void
		FadeoutWindow(Double transMilliSec, Byte fadeRatio, Byte opacityMin)
		{
			FadeWindow(transMilliSec, (SByte)(-fadeRatio), 255, opacityMin);
		}
		
		[MethodImpl(MethodImplOptions.Synchronized)]
		private void
		FadeWindow(Double transMilliSec, SByte fadeRatio, Byte opacityMaxLimit, Byte opacityMinLimit)
		{
			m_fadeRatio = fadeRatio;

#if FADE_USE_TIMER
			if (m_fadeTimer == null)
				m_fadeTimer = new System.Timers.Timer(transMilliSec);
			else
				m_fadeTimer.Interval = transMilliSec;

			m_fadeTimer.Elapsed += new ElapsedEventHandler(this.OnTimedEvent);
			m_fadeTimer.Enabled = true;
#else
			Byte i = m_opacity;//((m_fadeRatio > 0) ? (Byte)0 : m_opacity);
			while (m_fadeRatio + i < opacityMaxLimit && m_fadeRatio + i > opacityMinLimit) {
				i = (Byte)(i + m_fadeRatio);
				System.Threading.Thread.Sleep((Int32)transMilliSec);
				this.Update(i);
			}
			
			this.Update((Byte)((m_fadeRatio > 0) ? opacityMaxLimit : opacityMinLimit));
#endif
		}
		
#if FADE_USE_TIMER
		private void
		OnTimedEvent(Object sender, ElapsedEventArgs e)
		{
			if (m_fadeRatio + m_opacity < 255 && m_fadeRatio + m_opacity > 0) {
				i = (Byte)(i + m_fadeRatio);
				System.Threading.Thread.Sleep((Int32)transMilliSec);
				this.Update(i);
			} else {
				m_fadeTimer.Enabled = false;
				this.Update((Byte)((m_fadeRatio > 0) ? 255 : 0));
			}
		}
#endif

		private class Win32
		{
			public const Int32 GWL_EXSTYLE       = -20;
			public const Int32 WS_EX_LAYERED     = 0x00080000;
			public const Int32 WS_EX_TRANSPARENT = 0x00000020;
			public const Int32 AC_SRC_OVER   = 0x00000001;
			public const Int32 AC_SRC_ALPHA  = 0x00000001;
			
			[StructLayout(LayoutKind.Sequential)]
			public struct
			BlendFunction
			{
				public Byte BlendOp;
				public Byte BlendFlags;
				public Byte SourceConstantAlpha;
				public Byte AlphaFormat;
			}
			[StructLayout(LayoutKind.Sequential)]
			public struct
			Point
			{
				public Int32 x;
				public Int32 y;
			}
			[StructLayout(LayoutKind.Sequential)]
			public struct
			Size
			{
				public Int32 cx;
				public Int32 cy;
			}
			
			[DllImport("user32.dll")]
			public static extern Int32
			UpdateLayeredWindow(
				IntPtr            hWnd,
				IntPtr            hdcDst,
				ref Point         pptDst,
				ref Size          psize,
				IntPtr            hdcSrc,
				ref Point         pptSrc,
				UInt32            crKey,
				ref BlendFunction pblend,
				UInt32            dwFlags);

			[DllImport("user32.dll")]
			public static extern Int32
			GetWindowLong(IntPtr hWnd, Int32 nIndex);
			[DllImport("user32.dll")]
			public static extern Int32
			SetWindowLong(IntPtr hWnd, Int32 nIndex, Int32 dwNewLong);

			[DllImport("gdi32.dll")]
			public static extern IntPtr
			CreateCompatibleDC(IntPtr hDC);

			[DllImport("user32.dll")]
			public static extern IntPtr
			GetDC(IntPtr hWnd);

			[DllImport("user32.dll")]
			public static extern IntPtr
			ReleaseDC(IntPtr hWnd, IntPtr hDC);

			[DllImport("gdi32.dll")]
			public static extern Boolean
			DeleteDC(IntPtr hdc);

			[DllImport("gdi32.dll")]
			public static extern IntPtr
			SelectObject(IntPtr hDC, IntPtr hObject);

			[DllImport("gdi32.dll")]
			public static extern Boolean
			DeleteObject(IntPtr hObject);

			[DllImport("user32.dll")]
			public static extern Int32
			ReleaseCapture();
			
			[DllImport("user32.dll")]
			public static extern Int32
			SendMessage(IntPtr hWnd, Int32 wMsg, Int32 wParam, IntPtr lParam);
		} // Win32
	} // AlphaForm
} // Misuzilla.Windows.Forms
