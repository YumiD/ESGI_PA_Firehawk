using Grid.LevelEditor;
using UI.Models;

namespace UI.LevelEditor
{
    public class LevelEditorDropdown : AItemDropdown
    {
        private LevelEditorIconChoice.IconChoice _choiceDropdown = LevelEditorIconChoice.IconChoice.Tree; 
        public LevelEditorIconChoice.IconChoice ChoiceDropdown
        {
            get
            {
                ConvertToEnum();
                return _choiceDropdown;
            }
        }

        public override void ConvertToEnum()
        {
            _choiceDropdown = (LevelEditorIconChoice.IconChoice)CurrentIndex;
        }
    }
}