﻿#pragma checksum "C:\danieldev\github\mms-cortana\SpeechAndTTS\SynthesizeTextScenario.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "5AA4B55D1C0299D3172FD58F524D3C1E"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SpeechAndTTS
{
    partial class SynthesizeTextScenario : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                {
                    this.RootGrid = (global::Windows.UI.Xaml.Controls.Grid)(target);
                }
                break;
            case 2:
                {
                    this.StatusBlock = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 3:
                {
                    this.media = (global::Windows.UI.Xaml.Controls.MediaElement)(target);
                    #line 46 "..\..\..\SynthesizeTextScenario.xaml"
                    ((global::Windows.UI.Xaml.Controls.MediaElement)this.media).MediaEnded += this.media_MediaEnded;
                    #line default
                }
                break;
            case 4:
                {
                    this.textToSynthesize = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 5:
                {
                    this.btnSpeak = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 41 "..\..\..\SynthesizeTextScenario.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.btnSpeak).Click += this.Speak_Click;
                    #line default
                }
                break;
            case 6:
                {
                    this.listboxVoiceChooser = (global::Windows.UI.Xaml.Controls.ComboBox)(target);
                    #line 43 "..\..\..\SynthesizeTextScenario.xaml"
                    ((global::Windows.UI.Xaml.Controls.ComboBox)this.listboxVoiceChooser).SelectionChanged += this.ListboxVoiceChooser_SelectionChanged;
                    #line default
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

