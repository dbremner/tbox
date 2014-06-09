namespace Mnk.TBox.Tools.ConsoleUnitTestsRunner.Code.Contracts
{
    public interface IInfoView
    {
        void ShowArgs(CommandLineArgs cmd);
        void ShowLogo();
        void ShowHelp();
    }
}
