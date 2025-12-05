namespace namasdev.Core.IO
{
    public class File
    {
        public string Name { get; set; }
        public byte[] Content { get; set; }

        public bool IsEmpty
        {
            get { return Content == null || Content.Length == 0; }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
