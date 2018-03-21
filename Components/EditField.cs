using System;
using System.Collections.Generic;
using System.Text;

using KeyBoard = Xamarin.Forms.Keyboard;

namespace Xamarin.Forms.Consume.Simple.Components
{
    public class EditField : StackLayout
    {


        public KeyboardType Keyboard
        {
            get => KeyboardType.Default;
            set
            {
                if (Entry == null) return;
                switch (value)
                {
                    case KeyboardType.Default:
                        Entry.Keyboard = KeyBoard.Default;
                        break;
                    case KeyboardType.Chat:
                        Entry.Keyboard = KeyBoard.Chat;
                        break;
                    case KeyboardType.Email:
                        Entry.Keyboard = KeyBoard.Email;
                        break;
                    case KeyboardType.Numeric:
                        Entry.Keyboard = KeyBoard.Numeric;
                        break;
                    case KeyboardType.Plain:
                        Entry.Keyboard = KeyBoard.Plain;
                        break;
                    case KeyboardType.Telephone:
                        Entry.Keyboard = KeyBoard.Telephone;
                        break;
                    case KeyboardType.Text:
                        Entry.Keyboard = KeyBoard.Text;
                        break;
                    case KeyboardType.Url:
                        Entry.Keyboard = KeyBoard.Url;
                        break;
                }
            }
        }

        public KeyBoard KeyboardRaw
        {
            get => Entry?.Keyboard;
            set
            {
                if (Entry != null) Entry.Keyboard = value;
            }
        }


        public string Text
        {
            get => Entry?.Text ?? string.Empty;
            set
            {
                if (Entry != null) Entry.Text = value;
            }
        }

        public string Placeholder
        {
            get => Entry?.Placeholder ?? string.Empty;
            set
            {
                if (Entry != null) Entry.Placeholder = value;
            }
        }

        public bool IsPassword
        {
            get => Entry?.IsPassword ?? false;
            set
            {
                if (Entry != null) Entry.IsPassword = value;
            }
        }

        Entry Entry { get; }
        StackLayout Errors { get;  }

        public EditField()
        {
            Entry = new Entry();
            Children.Add(Entry);
            Errors = new StackLayout();
            Children.Add(Errors);
        }

    }
}
