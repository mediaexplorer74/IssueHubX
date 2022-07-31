using System.ComponentModel;
using IssueHub.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(LabelCell), typeof(IssueHub.UWP.Renderers.LabelCellRenderer))]

namespace IssueHub.UWP.Renderers
{
	public class LabelCellRenderer : TextCellRenderer
	{
		//
	}



	class NativeLabelCell : Cell//CellTableViewCell
	{
		//LabelCell LabelCell => Cell as LabelCell;

		public NativeLabelCell(Cell formsCell)
			//: base(UITableViewCellStyle.Value1, formsCell.GetType().FullName)
		{
			//Cell = formsCell;
		}

		//protected override void Dispose(bool disposing)
		//{
		//	if (disposing)
		//	{
		//		LabelCell.PropertyChanged -= CellPropertyChanged;
		//	}
		//	base.Dispose(disposing);
		//}

		public void UpdateCell()
		{
			UpdateText();
			UpdateDetail();
			UpdateAccessory();
		}

		public void CellPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == TextCell.TextProperty.PropertyName)
			{
				UpdateText();
			}
			else if (e.PropertyName == TextCell.DetailProperty.PropertyName)
			{
				UpdateDetail();
			}
			else if (e.PropertyName == LabelCell.AccessoryProperty.PropertyName)
			{
				UpdateAccessory();
			}
		}

		void UpdateText()
		{
			//TextLabel.Text = LabelCell.Text;
		}

		void UpdateDetail()
		{
			//DetailTextLabel.Text = LabelCell.Detail;
		}

		void UpdateAccessory()
		{
			//Accessory = GetNativeAccessory(LabelCell.Accessory);
		}

		/*TableViewCellAccessory GetNativeAccessory(CellAccessory accessory)
		{
		
			switch (accessory)
			{
				case CellAccessory.Checkmark:
					return UITableViewCellAccessory.Checkmark;
				case CellAccessory.DisclosureIndicator:
					return UITableViewCellAccessory.DisclosureIndicator;
				case CellAccessory.DetailDisclosureButton:
					return UITableViewCellAccessory.DetailDisclosureButton;
				case CellAccessory.DetailButton:
					return UITableViewCellAccessory.DetailButton;
				default:
					return UITableViewCellAccessory.None;
			}
		}
	}*/

	}//class end
}



