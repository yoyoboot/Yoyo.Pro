namespace Yoyo.Pro.Dtos
{
    public class ComboboxItemDtoT<T>
    {
       
        public ComboboxItemDtoT()
        {

        }

        public ComboboxItemDtoT(T value, string displayText)
        {
            this.Value = value;
            this.DisplayText = displayText;
        }

   
        public T Value { get; set; }

        public string DisplayText { get; set; }

        public bool IsSelected { get; set; }

    }
}
