// myTube.CategoryTextInfo

using RykenTube;
using Windows.UI.Xaml;

namespace myTube
{
  public class CategoryTextInfo : DependencyObject
  {
    public static readonly DependencyProperty TextProperty 
            = DependencyProperty.Register(nameof (Text), typeof (string), 
                typeof (CategoryTextInfo), new PropertyMetadata((object) ""));
    public static readonly DependencyProperty CategoryProperty
            = DependencyProperty.Register(nameof (Category), typeof (Category), 
                typeof (CategoryTextInfo), new PropertyMetadata((object) Category.All));

    public string Text
    {
      get => this.TextPath != null 
                ? App.Strings[this.TextPath, (string) this.GetValue(CategoryTextInfo.TextProperty)].ToLower() 
                : (string) this.GetValue(CategoryTextInfo.TextProperty);
      set => this.SetValue(CategoryTextInfo.TextProperty, (object) value);
    }

    public string RealText
    {
      get => this.TextPath != null 
                ? App.Strings[this.TextPath, (string) this.GetValue(CategoryTextInfo.TextProperty)].ToLower() 
                : (string) this.GetValue(CategoryTextInfo.TextProperty);
      set => this.SetValue(CategoryTextInfo.TextProperty, (object) value);
    }

    public string TextPath { get; set; }

    public Category Category
    {
      get => (Category) this.GetValue(CategoryTextInfo.CategoryProperty);
      set => this.SetValue(CategoryTextInfo.CategoryProperty, (object) value);
    }

    public override string ToString()
    {
        return this.RealText;
    }
  }
}
