namespace DotnetImgProxy
{
    public class ImageProxyOptions
    {
        public int? Width { get; set; }
        public int? Height { get; set; }
        public string Format { get; set; }
        public string ResizeType { get; set; } = "fill";
        public bool Enlarge { get; set; } = false;
        public bool Extend { get; set; } = false;
        public int Dpr { get; set; } = 2;
        public bool? AutoRotate { get; set; }
        public string Extra { get; set; } = null;
    }
}
