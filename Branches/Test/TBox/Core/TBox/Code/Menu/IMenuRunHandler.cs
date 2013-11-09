using WPFWinForms;

namespace TBox.Code.Menu
{
	interface IMenuRunHandler
	{
		void Handle(UMenuItem item, string[]path);
	}
}
