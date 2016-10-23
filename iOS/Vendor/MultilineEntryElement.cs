//
// MultilineEntryElement.cs: multi-line element entry for MonoTouch.Dialog
// 
// Author:
// Aleksander Heintz (alxandr@alxandr.me)
// Based on the code for the EntryElement by Miguel de Icaza
//

using System;
using Foundation;
using UIKit;
using System.Drawing;
using MonoTouch.Dialog;
using CoreGraphics;

namespace Shoperella.iOS
{
	public class MultilineEntryElement : UIViewElement
	{
		public MultilineEntryElement(string placeholder, float height, bool transparentBackground)
			: base(null, new MultilineView(placeholder, height, transparentBackground), false)
		{
			Flags = CellFlags.DisableSelection;
			if (transparentBackground)
				Flags |= CellFlags.Transparent;
		}

		/// <summary>
		/// The key used for reusable UITableViewCells.
		/// </summary>
		private static readonly NSString EntryKey = new NSString("MultilineEntryElement");

		protected override NSString CellKey
		{
			get { return EntryKey; }
		}

		public override void Selected(DialogViewController dvc, UITableView tableView, NSIndexPath path)
		{
			tableView.DeselectRow(path, false);
			foreach (var v in View.Subviews)
			{
				if (v.GetType() == typeof(UITextView))
					v.BecomeFirstResponder();
			}
			//View.Subviews.FindControlOfType<UITextView>().BecomeFirstResponder();
		}

		public override string Summary()
		{
			UITextView tv = View.Subviews[0] as UITextView;
			if (tv != null)
				return tv.Text;
			return null;
		}

		private class TextDelgate : UITextViewDelegate
		{
			private string _placeholder;

			public TextDelgate(string placeholder)
			{
				_placeholder = placeholder;
			}

			public override void Changed(UITextView textView)
			{
				if (textView.TextColor == UIColor.LightGray)
				{
					textView.Text = textView.Text.Substring(0, 1);
					textView.TextColor = UIColor.Black;
				}
				else if (textView.Text.Length == 0)
				{
					textView.Text = _placeholder;
					textView.TextColor = UIColor.LightGray;
					textView.SelectedRange = new NSRange(0, 0);
				}
			}

			public override void SelectionChanged(UITextView textView)
			{
				if (textView.TextColor == UIColor.LightGray && (textView.SelectedRange.Location != 0 || textView.SelectedRange.Length != 0))
				{
					textView.SelectedRange = new NSRange(0, 0);
				}
			}
		}

		private class MultilineView : UIView
		{
			public MultilineView(string placeholder, float height, bool transparentBackground)
			{
				//Temporary width until we can re-layout
				float containerWidth = 10;

				// create actual text view
				UITextView textView = new UITextView(new RectangleF(0, 0, containerWidth, height - (transparentBackground ? 0 : 12)))
				{
					AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleLeftMargin,
					Text = placeholder,
					TextAlignment = UITextAlignment.Left,
					TextColor = UIColor.LightGray,
					Delegate = new TextDelgate(placeholder),
				};

				AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleRightMargin;

				base.Frame = new RectangleF(transparentBackground ? 0 : 3, transparentBackground ? 0 : 2, containerWidth, height);

				if (transparentBackground)
				{
					base.BackgroundColor = UIColor.Clear;
					textView.Layer.BackgroundColor = UIColor.White.CGColor;
					textView.Layer.BorderWidth = 1f;
					textView.Layer.BorderColor = UIColor.LightGray.CGColor;
					textView.Layer.CornerRadius = 8f;
				}
				base.AddSubview(textView);
			}

			public override void LayoutSubviews()
			{
				var superWidth = Superview.Superview.Frame.Width;
				Frame = new RectangleF((float)Frame.X, (float)Frame.Y, (float)(superWidth - Frame.X), (float)Frame.Height);

				var subFrame = Subviews[0].Frame;
				Subviews[0].Frame = new RectangleF((float)subFrame.X, (float)subFrame.Y, (float)(superWidth - (Frame.X * 3)), (float)subFrame.Height);
				Subviews[0].LayoutSubviews();
			}
		}
	}
}
