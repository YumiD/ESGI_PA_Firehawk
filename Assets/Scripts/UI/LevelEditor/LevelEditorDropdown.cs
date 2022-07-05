using Grid.Models;
using UI.Models;

namespace UI.LevelEditor
{
    public class LevelEditorDropdown : AItemDropdown<AIconChoice.IconChoice>
    {
        public override void ResetValue()
        {
            ChoiceDropdown = AIconChoice.IconChoice.Default;
        }

        protected override void ConvertToEnum()
        {
            ChoiceDropdown = (AIconChoice.IconChoice)CurrentIndex;
        }

        public void OnSelect()
        {
            ItemDropdown.onValueChanged.Invoke(0);
        }
    }
}