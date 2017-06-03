namespace Backend.Models
{
	public class Form
	{
		public FormMode Mode { get; set; }
		public string Title { get; set; }
		public string Message { get; set; }
	}

	public enum FormMode
	{
		Bug = 0,
		Feature = 1
	}
}