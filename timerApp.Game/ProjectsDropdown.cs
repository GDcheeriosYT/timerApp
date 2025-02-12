using System;
using osu.Framework.Bindables;
using osu.Framework.Graphics.UserInterface;

namespace timerApp.Game
{
    public partial class ProjectsDropdown : BasicDropdown<string>
    {
        public ProjectsDropdown(Bindable<Projects> projects)
        {
            // Current = new Bindable<string>("Test");

            foreach (Projects project in Enum.GetValues(typeof(Projects)))
            {
                AddDropdownItem(project.ToString());
            }
        }
    }
}
