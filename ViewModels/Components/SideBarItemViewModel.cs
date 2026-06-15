using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;

namespace SongsInLearning.ViewModels

{
    public class SideBarItemViewModel
    {
        public bool IsCategory { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;
        public int PrivilegeId { get; set; } = 0;
        public bool Enabled { get; set; } = true;
        public string IconKey { get; set; }

        public ICollection<SideBarItemViewModel> Children { get; set; } = new List<SideBarItemViewModel>();
        public IRelayCommand<string>? NavigateCommand { get; set; }



        public SideBarItemViewModel()
        {
            
        }

        public SideBarItemViewModel(string title, ICollection<SideBarItemViewModel> children)
        {
            IsCategory = true;
            Title = title;
            Children = children;
        }

        public SideBarItemViewModel(string key, string title, int privilegeId, bool enabled, string iconKey)
        {
            IsCategory = false;
            Key = key;
            Title = title;
            PrivilegeId = privilegeId;
            Enabled = enabled;
            IconKey = iconKey;
        }
    }
}
