﻿#pragma checksum "..\..\..\Windows\PolygonWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "9BFF06B714C2F8369A763703BBB28125439D4D5057B841AB82A6C500307C9063"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using ProjektniZadatak1.Windows;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace ProjektniZadatak1.Windows {
    
    
    /// <summary>
    /// PolygonWindow
    /// </summary>
    public partial class PolygonWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 31 "..\..\..\Windows\PolygonWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtBoxBorderThickness;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\..\Windows\PolygonWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cmbBoxBorderColor;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\..\Windows\PolygonWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cmbBoxFillColor;
        
        #line default
        #line hidden
        
        
        #line 55 "..\..\..\Windows\PolygonWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtBoxText;
        
        #line default
        #line hidden
        
        
        #line 63 "..\..\..\Windows\PolygonWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cmbBoxTextColor;
        
        #line default
        #line hidden
        
        
        #line 71 "..\..\..\Windows\PolygonWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox chkBoxTransparent;
        
        #line default
        #line hidden
        
        
        #line 89 "..\..\..\Windows\PolygonWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnExecute;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/ProjektniZadatak1;component/windows/polygonwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Windows\PolygonWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.txtBoxBorderThickness = ((System.Windows.Controls.TextBox)(target));
            return;
            case 2:
            this.cmbBoxBorderColor = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 3:
            this.cmbBoxFillColor = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 4:
            this.txtBoxText = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.cmbBoxTextColor = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 6:
            this.chkBoxTransparent = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 7:
            this.btnExecute = ((System.Windows.Controls.Button)(target));
            
            #line 87 "..\..\..\Windows\PolygonWindow.xaml"
            this.btnExecute.Click += new System.Windows.RoutedEventHandler(this.Draw_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            
            #line 95 "..\..\..\Windows\PolygonWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Cancel_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

