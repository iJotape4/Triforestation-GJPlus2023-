public class ClearHHashesOnExitPanicButton : UIButton
{
    protected override void ClickButtonMethod()
    {
        TriangularGrid.ClearHashes();
    }
}