using System;
using System.Collections.Generic;
using System.IO;

namespace namasdev.Core.IO
{
	public class FileContentTypes
	{
		public class Image
		{
            public const string BMP = "image/bmp";
            public const string GIF = "image/gif";
			public const string JPEG = "image/jpeg";
			public const string PNG = "image/png";
			public const string TIFF = "image/tiff";
            public const string SVG = "image/svg+xml";
        }

		public class Video
		{
			public const string MPEG = "video/mpeg";
            public const string MP4 = "video/mp4";
        }

		public class Text
		{
			public const string CSV = "text/plain";
			public const string HTML = "text/html";
			public const string TXT = "text/plain";
			public const string XML = "text/xml";
            public const string JSON = "text/json";
        }

		public class Application
		{
			public const string EXCEL = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            public const string EXCEL_97_2003 = "application/vnd.ms-excel";
            public const string POWERPOINT = ".pptx";
            public const string POWERPOINT_97_2003 = ".ppt";
            public const string WORD = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            public const string WORD_97_2003 = "application/msword";
            public const string PDF = "application/pdf";
            public const string ZIP = "application/x-zip-compressed";
            public const string RAR = "application/vnd.rar";
            public const string DEFAULT = "application/octet-stream";
        }

		private static readonly Dictionary<string, string> _contentTypesByExtensions = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase)
		{
            { FileExtensions.Image.BMP,     Image.BMP   },
            { FileExtensions.Image.GIF,     Image.GIF   },
			{ FileExtensions.Image.JPG,     Image.JPEG  },
			{ FileExtensions.Image.JPE,     Image.JPEG  },
			{ FileExtensions.Image.JPEG,    Image.JPEG  },
			{ FileExtensions.Image.PNG,     Image.PNG   },
			{ FileExtensions.Image.TIFF,	Image.TIFF  },
			{ FileExtensions.Image.TIF,     Image.TIFF  },
            { FileExtensions.Image.SVG,     Image.SVG  },

            { FileExtensions.Video.MPG,		Video.MPEG  },
			{ FileExtensions.Video.MPE,		Video.MPEG  },
			{ FileExtensions.Video.MPEG,	Video.MPEG  },
            { FileExtensions.Video.MP4,    Video.MPEG  },

            { FileExtensions.Text.CSV,		Text.CSV	},
			{ FileExtensions.Text.HTML,		Text.HTML	},
			{ FileExtensions.Text.TXT,		Text.TXT	},
			{ FileExtensions.Text.XML,		Text.XML	},
            { FileExtensions.Text.JSON,		Text.JSON	},

			{ FileExtensions.Application.EXCEL,					Application.EXCEL				},
			{ FileExtensions.Application.EXCEL_97_2003,			Application.EXCEL_97_2003		},
			{ FileExtensions.Application.POWERPOINT,			Application.POWERPOINT			},
            { FileExtensions.Application.POWERPOINT_97_2003,	Application.POWERPOINT_97_2003	},
            { FileExtensions.Application.WORD,					Application.WORD				},
            { FileExtensions.Application.WORD_97_2003,			Application.WORD_97_2003		},
			{ FileExtensions.Application.PDF,					Application.PDF					},
			{ FileExtensions.Application.ZIP,					Application.ZIP					},
            { FileExtensions.Application.RAR,					Application.RAR					},
        };

		public static string GetContentTypeByFileName(string fileName)
		{
			if (string.IsNullOrWhiteSpace(fileName))
			{
				throw new ArgumentNullException(nameof(fileName));
			}

			string contentType;
			return _contentTypesByExtensions.TryGetValue(Path.GetExtension(fileName), out contentType)
				? contentType
				: Application.DEFAULT;
		}
	}
}