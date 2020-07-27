using System;
using System.Collections.Generic;
using System.Text;

namespace FIT_PONG.Mobile.Models
{
    public enum MenuItemType
    {
        Browse,
        About,
        Naslovnica,
        Profil,
        Igrači,
        Takmičenja,
        Chat,
        Reports,
        Logout

    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
