using UnityEngine;
using UnityEngine.UIElements;

namespace LocalizationModule.Editor
{
    [UxmlElement]
    internal partial class UxmlImage : Image
	{
		#region Constructors
		
		public UxmlImage() : base()
		{
			
		}

		#endregion

		#region Properties

		[UxmlAttribute]
		public new Texture image
		{
			get => base.image;
			set => base.image = value;
		}

		[UxmlAttribute]
		public new Sprite sprite
		{
			get => base.sprite;
			set => base.sprite = value;
		}

		#endregion
	}
}