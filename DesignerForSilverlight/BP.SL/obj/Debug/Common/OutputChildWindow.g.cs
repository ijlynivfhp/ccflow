﻿#pragma checksum "D:\ccflow\DesignerForSilverlight\BP.SL\Common\OutputChildWindow.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "4618EB224C2D9FF14A771343AEB424A3"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.1022
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using SilverlightFX.UserInterface;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace BP.SL {
    
    
    public partial class OutputChildWindow : System.Windows.Controls.ChildWindow {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBox log;
        
        internal SilverlightFX.UserInterface.MouseWheelScroll mouseWheel;
        
        internal System.Windows.Controls.Button ClearButton;
        
        internal System.Windows.Controls.Button OKButton;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/BP.SL;component/Common/OutputChildWindow.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.log = ((System.Windows.Controls.TextBox)(this.FindName("log")));
            this.mouseWheel = ((SilverlightFX.UserInterface.MouseWheelScroll)(this.FindName("mouseWheel")));
            this.ClearButton = ((System.Windows.Controls.Button)(this.FindName("ClearButton")));
            this.OKButton = ((System.Windows.Controls.Button)(this.FindName("OKButton")));
        }
    }
}
