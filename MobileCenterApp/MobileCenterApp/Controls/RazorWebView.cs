using System;
using Xamarin.Forms;

namespace MobileCenterApp
{
	public class RazorWebView : WebView
	{
		public static readonly BindableProperty RazorTemplateProperty = BindableProperty.Create(nameof(RazorTemplate), typeof(object), typeof(RazorWebView), null);
		public object RazorTemplate
		{
			get { return GetValue(RazorTemplateProperty); }
			set { SetValue(RazorTemplateProperty, value); }
		}


		public static readonly BindableProperty ModelProperty = BindableProperty.Create(nameof(Model), typeof(object), typeof(RazorWebView), null);
		public object Model
		{
			get { return GetValue(ModelProperty); }
			set { 
				SetValue(ModelProperty, value);
				OnPropertyChanged(nameof(Model));
			}
		}

		protected override void OnPropertyChanged(string propertyName)
		{
			base.OnPropertyChanged(propertyName);
			if (propertyName != nameof(Model) && propertyName != nameof(RazorTemplate))
				return;
			Source = new HtmlWebViewSource { Html = GetHtml() };
		}

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();
			Source = new HtmlWebViewSource { Html = GetHtml() };
		}
		public string GetHtml()
		{
			try
			{
				if (RazorTemplate == null)
					return "No Template";
				var template = RazorTemplate;
				var model = Model;
				if (model != null)
				{
					var modelProperty = template.GetType().GetProperty("Model");
					if (modelProperty != null)
						modelProperty.SetValue(template, model, null);
				}

				var method = template.GetType().GetMethod("GenerateString");
				if (method == null)
					return "The template doesnt implement GenerateString";
				var html = (string)method.Invoke(template, null);
				return html;
			}
			catch (Exception ex)
			{
				return ex.Message;
			}

		}
	}
}
