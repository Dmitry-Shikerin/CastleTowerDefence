
****************************************
Overview
****************************************
For more complete and up to date docs as well as tutorial videos please visit the web site at http://www.west-racing.com/mf

A system of mesh modifiers for Unity written in C# and making use of Burst and Jobs for big speed increases. The modifiers included can be applied individually or multiple mods can be added to a mesh based object in Unity. The system is inspired by the 3ds max system so users of max should find most of the functionality and params familiar, and for others there is no coding required just setting of params to alter effects. Please note this is currently in beta so there could well be some issues that need sorting out. There may be a slight mismatch on Axis on some modifiers, the system was converted over from code for our own engine where Z was up.

I have other modifiers almost ready to be added and I will add these very shortly, they include FFD Cyl, FFD Box, Path deform and also a system of World Space modifiers such as ripple and wave. These will be added as updates to the system in the next few weeks. I may also add our code for exporting modifiers from Max if there is a need for it. It would be possible for the user to add their own Modifiers to the system and I will probably expose that shortly as well. And if anyone has anything they need then please feel free to email me at chris@west-racing.com.

All code is copyright Chris West 2021 and any use outside of the Unity engine is not permitted.

****************************************
How to use:
****************************************
To add a modifier to a mesh you first need to add the ModifyObject.cs component to the object then add the Modifiers you require. The ModifyObject.cs script is the manager for any modifiers on the object, it allows multiple modifiers to be stacked on top of one another to achieve different end results. Once the manager is in place you can use the drop down box above the Quick Edit area in the inspector at add and modifier from the list.
The modifier will be added as a component to the object, you can either edit the values in the component part of the inspector like any other Unity component, or you can edit the most common values for each modifier in the Quick Edit area of the inspector.
This area also allows you to quickly disable any modifier with the check box on the left, or delete the modifier with  the button on the right. Click the Name of the modifier top open and close the param section.
The order the modifiers are applied is quite important and you can change this at any time by dragging the components in the inspector.
Each modifier is represented by a yellow box and the effect of the modifier is shown by this box so you can judge what will happen at runtime. If a modifier can be constrained to an area red and green boxes will show the extents of the constraint.

Each modifiers space is controlled by the gizmo pos, rot and scale params, usually you can leave these at default values of 0, but in case an effect doesnt quite happen in the way you could play with these values to get the desired result.

If you arnt planning on having any modifications happen in game and are just deforming meshes then you can delete all the modifier scripts once you are happy with a deformation and just be left with a deformed mesh.

****************************************
Modifier
****************************************
All the modifiers share some common params.
Class:
public class Modifier : MonoBehaviour
{
	public bool			ModEnabled;
	public bool			DisplayGizmo;
	public int			Order;
	public Vector3		Offset;
	public Vector3		gizmoPos;
	public Vector3		gizmoRot;
	public Vector3		gizmoScale;
}

Mod Enabled		-	Turns the modifier on/off
Display Gizmo	-	Shows the box gizmo for the modifier, usually you will have this on but if you have multiple							modifiers on a mesh it can get confusing.
Order			-	When more than modifier is in the stack the order they are processed in can be important, so a							modifier with Order value of -1 will be processed before one of Order 0
Offset			-	Offset added to each vertex before the modifier is applied, the effect of this depends on the modifier.
Gizmo Pos		-	A local offset added to the vertex, again its effect varies on the modifier in use.
Gizmo Rot		-	A local rotation of the modifier space.
Gizmo Scale		-	A local scale of the modifier space.

****************************************
ModifyObject
****************************************
ModifyObject is derived from Modifiers.

Class:
public class Modifiers : MonoBehaviour
{
	public bool			recalcnorms;
	public bool			recalcbounds;
	public bool			Enabled;
	public bool			DoLateUpdate;
	public bool			GrabVerts;
}

public class ModifyObject : Modifiers
{
}

Param description:
Enabled			-	When enabled the modifier stack is evaluated to get the final vertex positions, if not enabled the						last calculated positions will be used, so generally you will clear this when you have calculated a						desired	effect on a mesh to stop further calculations being done, and you will enable again if							further modifications need to be made.
RecalcNorms		-	Tell the system to recalculate normals for the mesh after the stack has been evaluated.
RecalcBounds	-	Tell the system to recalulate the bounds info for the mesh.
DoLateUpdate	-	Set if you require the modifier stack to be evaluated and applied in the LateUpdate instead of							Update
GrabVerts		-	Grabs the mesh verts again at start of modify. Use if something higher up the pipeline alters the						mesh vertices.

Modifiers:

****************************************
Bend:
****************************************
The Bend modifier lets you bend the current selection about a single axis, producing a uniform bend in an object's geometry. You can control the angle and direction of the bend on any of three axes. You can also limit the bend to a section of the geometry.

Class:
public class Bend : Modifier
{
	public float		angle;
	public float		dir;
	public Axis			axis;
	public bool			doRegion;
	public float		from;
	public float		to;
}

Param description:
Angle			- Sets the angle to bend from the viertical plane.
Direction		- Sets the direction of the bend relative to the horizontal plane.
Axis			- Specifies the axis to be bent.
DoRegion		- Limits the effect to a region of the mesh. The area of the effect will be shown by red and green boxes.
From			- Lower boundary of the effect
To				- Upper boundary of the effect

****************************************
Bubble:
****************************************
The Bubble modifier will inflate or pinch an object according to the transformation of the modifier's gizmo, the radius and falloff parameters.

Class:
public class Bubble : Modifier
{
	public float		radius;
	public float		falloff;
}

Param description:
Radius			-	Is the length of the offset a vertex would get if it's distance from the gizmo evaluates as zero. This						parameter can be set to a negative value as well to create a pinch effect.
Falloff			-	Determines how far from the gizmo will the bubble effect the deformation.

****************************************
Hump:
****************************************
Hump is a simpleMod scripted plug-in modifier which folding an object according to the transformation of the modifier's gizmo, the amount and heaping parameters over X, Y or Z axis.

Class:
public class Hump : Modifier
{
	public float		amount;
	public float		cycles;
	public float		phase;
	public bool			animate;
	public float		speed;
	public Axis			axis;
}

Param description:
Amount			-	Amount of hump
Cycles			-	Number of humps to apply
Phase			-	Shifts the wave
Animate			-	Animate the effect
Speed			-	Speed of animation
Axis			-	Axis to apply effect to

****************************************
Melt:
****************************************
The Melt modifier lets you apply a realistic melting effect to the mesh. Options include sagging of edges, spreading while melting, and a customizable set of substances ranging from a firm plastic surface to a jelly type that collapses in on itself.

Class:
public class Melt : Modifier
{
	public float		Amount;
	public float		Spread;
	public MeltMat		MaterialType;
	public float		Solidity;
	public Axis			axis;
	public bool			FlipAxis;
}

Param description:
Amount			-	Specifies the extent of the "decay," or melting effect applied to the gizmo, thus affecting the object.
Spread			-	Specifies how much the object and melt will spread as the Amount value increases. It's basically a "bulge"					along a	flat plane.
Material Type	- 	Ice. The default Solidity setting.
					Glass. Uses a high Solidity setting to simulate glass.
					Jelly. Causes a significant drooping effect in the center.
					Plastic. Relatively solid, but droops slightly in the center as it melts.
					Custom. User set value.

Solidity		-	If material is Custom sets how solid the mesh is.
Axis			-	Choose the axis on which the melt will occur.

Flip Axis		-	Normally, the melt occurs from the positive direction toward the negative along a given axis. Turn on Flip					Axis to	reverse this direction.

****************************************
Noise:
****************************************
The Noise modifier modulates the position of an object's vertices along any combination of three axes. This modifier simulates random variations in an object's shape. Using a fractal setting, you can achieve random, rippling patterns, like a flag in the wind. The results of the Noise modifier are most noticeable on objects that have greater numbers of faces.

Class:
public class Noise : Modifier
{
	public int			Seed;
	public float		Scale;
	public bool			Fractal;
	public float		Freq;
	public float		Iterations;
	public bool			Animate;
	public float		Phase;
	public float		Rough;
	public Vector3		Strength;
}

Param description:
Seed			-	Generates a random start point from the number you set.
Scale			-	Sets the size of the noise effect (not strength). Larger values produce smoother noise, lower values more					jagged noise
Strength		-	Set the strength of the noise effect along each of three axes. Enter a value for at least one of these						axes to produce a noise effect.
Animate			-	Regulates the combined effect of Noise and Strength parameters. The following parameters adjust the							underlying wave.
Freq			-	Sets the periodicity of the sine wave. Regulates the speed of the noise effect. Higher frequencies make						the noise quiver faster. Lower frequencies produce a smoother and more gentle noise.
Phase			-	Shifts the start and end points of the underlying wave.
Fractal			-	Produces a fractal effect based on current settings.

If you turn on Fractal, the following options are available: 
Rough			-	Determines the extent of fractal variation. Lower values are less rough than higher values.
Iterations		-	Controls the number of iterations (or octaves) used by the fractal function. Fewer iterations use less						fractal	energy and generate a smoother effect. An iteration of 1.0 is the same as turning Fractal off.

****************************************
Push:
****************************************
The Push modifier lets you "push" object vertices outward or inward along the average vertex normals. This produces an "inflation" effect.

Class:
public class Push : Modifier
{
	public float		amount;
	public NormType		method;
}

Param description:
Amount			-	How far to push vertices.
Method			-	Leave as normals for now, other values are being tested.

****************************************
RadialSkew:
****************************************
Radial Skew skews objects radially instead of along a single axis.

Class:
public class RadialSkew : Modifier
{
	public float		angle;
	public Axis			axis;
	public Axis			eaxis;
	public bool			biaxial;
}

Param description:
Angle			-	Angle of the Skew
Axis			-	Major axis of the skew
EAxis			-	Effective axis of the skew
BiAxial			-	This is similar to what Taper produces when both its primary and effect axes are set to single axes, but					here, the points are moved depending on which side of the center axis they lie along the effect axis. And,					again, the adjustment is described with an angular measurement, not a percentage.

****************************************
Ripple:
****************************************
The Ripple modifier lets you produce a concentric rippling effect in an object's geometry. You can set either of two ripples or a combination of both.

Class:
public class Ripple : Modifier
{
	public float		amp;
	public float		amp2;
	public float		flex;
	public float		wave;
	public float		phase;
	public float		Decay;
	public bool			animate;
}

Param description:
Amp/ Amp 2		-	Amplitude 1 produces a ripple across the object in one direction, while Amplitude 2 creates a similar						ripple at right angles to the first (that is, rotated 90 degrees about the vertical axis).
Wave			-	Specifies the distance between the peaks of the wave. The greater the length, the smoother and more							shallow the ripple for a given amplitude.
Phase			-	Shifts the ripple pattern over the object. Positive numbers move the pattern inward, while negative							numbers move it outward. This effect becomes especially clear when animated.
Decay			-	Limits the effect of the wave generated from its center. The value of 0.0 means that the wave will							generate infinitely from its center. Increasing the Decay value causes the wave amplitudes to decrease						with distance from the center, thus limiting the distance over which the waves are generated.
Animate			-	Animate the ripple
Radius			-	Radius of the gizmo
Segments		-	Number of segments in the gizmo
Circles			-	Number of circles in the gizmo

****************************************
Skew:
****************************************
The Skew modifier lets you produce a uniform offset in an object's geometry. You can control the amount and direction of the skew on any of three axes. You can also limit the skew to a section of the geometry.

Class:
public class Skew : Modifier
{
	public float		amount;
	public bool			doRegion;
	public float		to;
	public float		from;
	public float		dir;
	public Axis			axis;
}

Param description:
Amount			-	Sets the angle to skew from the vertical plane.
Direction		-	Sets the direction of the skew relative to the horizontal plane.
Axis			-	Specify the axis that will be skewed.
Do Region		-	Constrain modifier to a region
From			-	Start of region
To				-	End of region

****************************************
Spherify:
****************************************
The Spherify modifier distorts an object into a spherical shape. This modifier has only one parameter: a Percent spinner that deforms the object, as much as possible, into a spherical shape. The success of the operation depends on the topology of the geometry to which it's applied. For example, a cylinder with no height segments will result in little change. Adding height segments will result in a barrel at 100 percent. Adding cap segments will produce a sphere.

Class:
public class Spherify : Modifier
{
	public float		percent;
}

Param description:
Percent			-	Sets the percentage of spherical distortion to apply to an object.

****************************************
Stretch:
****************************************
The Stretch modifier simulates the traditional animation effect of "squash-and-stretch." Stretch applies a scale effect along a specified stretch axis and an opposite scale along the two remaining minor axes. The amount of opposite scaling on the minor axes varies, based on distance from the center of the scale effect. The maximum amount of scaling occurs at the center and falls off toward the ends.

Class:
public class Stretch : Modifier
{
	public float		amount;
	public bool			doRegion;
	public float		to;
	public float		from;
	public float		amplify;
	public Axis			axis;
}

Param description:
Amount			-	Sets the base scale factor for all three axes. The scale factor derived from the Stretch value varies						according to the sign of the value.

						Positive stretch values define a scale factor equal to Stretch+1. For example, a stretch value of 1.5						yields a scale factor of 1.5+1=2.5, or 250 percent.
					
						Negative stretch values define a scale factor equal to -1/(Stretch-1). For example, a stretch value of						-1.5 yields a scale factor of -1/(-1.5-1)=0.4, or 40 percent.
					
					The calculated scale factor is applied to the selected stretch axis and the inverse scale is applied to						the minor axes.

Amplify			-	Changes the scale factor applied to the minor axes. Amplify generates a multiplier using the same							technique as stretch. The multiplier is then applied to the Stretch value before the scale factor for the					minor axes is calculated.	Amplify values affect scaling along the minor axes in the following way:
						A value of 0 has no effect. It uses the default scale factor calculated from the Stretch amount.
						Positive values exaggerate the effect.
						Negative values reduce the effect.
Axis			-	Axis to stretch along
Do Region		-	Constrain the effect to a region
From			-	Start of the region
To				-	End of the region

****************************************
Taper:
****************************************
The Taper modifier produces a tapered contour by scaling both ends of an object's geometry; one end is scaled up, and the other is scaled down. You can control the amount and curve of the taper on two sets of axes. You can also limit the taper to a section of the geometry.

Class:
public class Taper : Modifier
{
	public float		amount;
	public bool			doRegion;
	public float		to;
	public float		from;
	public float		dir;
	public Axis			axis;
	public EffectAxis	EAxis;
	public float		crv;
	public bool			sym;
}

Param description:
Amount			-	The extent to which the ends are scaled.
Crv				-	Applies a curvature to the sides of the Taper gizmo, thus affecting the shape of the tapered object.						Positive values produce an outward curve along the tapered sides, negative values an inward curve. At 0,					the sides are unchanged.
Dir				-	Swivel amount around axis.
Axis			-	The central axis or spine of the taper: X, Y, or Z.
EAxis			-	The axis, or pair of axes, indicating the direction of the taper from the primary axis. The available						choices are determined by the choice of primary axis.
Sym				-	Produces a symmetrical taper around the primary axis. A taper is always symmetrical around the effect axis
Do Region		-	Limit effect to a region.
From			-	Start of region.
To				-	End of region.

****************************************
Twist:
****************************************
The Twist modifier produces a twirling effect (like wringing out a wet rag) in an object's geometry. You can control the angle of the twist on any of three axes, and set a bias that compresses the twist effect relative to the pivot point. You can also limit the twist to a section of the geometry.

Class:
public class Twist : Modifier
{
	public float		angle;
	public bool			doRegion;
	public float		from;
	public float		to;
	public float		Bias;
	public Axis			axis;
}

Param description:
Angle			-	Determines the amount of twist around the vertical axis.
Bias			-	Causes the twist rotation to bunch up at either end of the object. When the parameter is negative, the					object twists closer to the gizmo center. When the value is positive, the object twists more away from					the gizmo center. When the parameter is 0, the twisting is uniform.
Axis			-	Specify the axis along which the twist will occur. This is the local axis of the Twist gizmo.
Do Region		-	Limit effect to a region of the mesh
From			-	Start of the region
To				-	End of the region.

****************************************
Wave:
****************************************
The Wave modifier produces a wave effect in an object's geometry. You can use either of two waves, or combine them. Wave uses a standard gizmo and center, which you can transform to increase the possible wave effects.

Class:
public class Wave : Modifier
{
	public float		amp;
	public float		amp2;
	public float		flex;
	public float		wave;
	public float		phase;
	public float		Decay;
	public float		dir;
	public bool			animate;
}

Param description:
Amp/Amp 2		-	Amplitude 1 produces a sine wave along the gizmo's Y axis, while Amplitude 2 creates a wave along the X					axis (although peaks and troughs appear in the same direction with both). Switching a value from						positive to negative reverses the positions of peaks and troughs.
Wave			-	Specifies the distance in current units between the crests of both waves.
Phase			-	Shifts the wave pattern over the object. Positive numbers move the pattern in one direction, while						negative numbers move it in the other. This effect is especially clear when animated.
Decay			-	Limits the effect of the wave generated from its origin. A decay value decreases the amplitude at						increasing distance from the center. As this value increases, the wave is concentrated at the center					and flattened until it disappears (completely decays).
Dir				-	Direction of wave
Animate			-	Animate the wave
Divs			-	Number of divisions in the gizmo
NumSegs			-	Number of segements in the gizmo
NumSides		-	Number of sides in the gizmo

****************************************
FFD
****************************************
FFD stands for Free-Form Deformation. The FFD modifier surrounds the selected geometry with a lattice. By adjusting the control points of the lattice, you deform the enclosed geometry.

There are three FFD modifiers, each providing a different lattice resolution: 2x2, 3x3, and 4x4. The 3x3 modifier, for example, provides a lattice with three control points across each of its dimensions or nine on each side of the lattice.

Class:
public class FFD : Modifier
{
	public bool			DrawGizmos;
	public float		KnotSize;
	public bool			inVol;
	public Vector3[]	pt;
}

Param Description:
DrawGizmos		-	Draw the control points and gizmo in edit mode, allows user to drag points
KnotSize		-	Relationship of knotsize to lattice grid size
inVol			-	Only deform points that lie inside the source volume.
pt				-	Array of 64 vector3 which hold the control points, for 2x2x2 first 8 are used, 3x3x3 first 27

****************************************
Displace
****************************************
Displace vertices based on their normal and a texture lookup for the amount.

Class:
public class Displace : Modifier
{
	public Texture2D	map;
	public float		amount;
	public Channel		channel;
	public Vector2		offset;
	public Vector2		scale;
}

Param Description:
map				-	Texture to control the displacement, uv coords for a vertex govern the lookup pos. Texture must be
					readable.
amount			-	Amount to displace vertex which is modulated by the texture lookup.
channel			-	Channel of the texture to use for the displacment.
offset			-	UV offset, use for scrolling.
scale			-	Scale uv coords.

****************************************
Bulge
****************************************
Bulge behaves like spherify but you have control over each axis and with a falloff per axis.

Class:
public class Bulge : Modifier
{
	public Vector3	Amount;
	public Vector3	FallOff;
	public bool		LinkFallOff;
}

Param Description:
Amount			-	Amount of bulge on each axis.
FallOff			-	Falloff of the bulge on each axis.
LinkFallOff		-	Link all falloff values to the x axis value

****************************************
Cylindrify
****************************************
This is like Spherify but only acts on 2 axis so a mesh will deform towards a cylinder.

Class:
public class Cylindrify : Modifier
{
	public float Percent = 0.0f;
	public float Decay = 0.0f;
}

Param Description:
Percent			-	Amount of transform towards a Cylinder
Decay			-	How quickly the effect fades with distance.

****************************************
Page Flip
****************************************
Start of a page turning system. This one is very much wip in progress, it works but there is room for improvement, I hope
to add a page mesh generator and also a proper book system. Iam not going to document this yet as it really is WIP.


****************************************
UV Adjust
****************************************
This is a quick general purpose UV modifier, you can change the uv offset, tiling amount and rotation of uvs. This will be improved in later versions to work on submeshes and other features.

Class:
public class UVAdjust : Modifier
{
	public Vector3		gizmoPos;
	public Vector3		gizmoRot;
	public Vector3		gizmoScale;
	public Vector3		Offset;
	public bool			animate;
	public float		rotspeed;
	public float		spiralspeed;
	public Vector3		speed;
	public float		spiral;
	public float		spirallim;
}

Param Description:
gizmoPos		-	Amount to add to uv coordinates
gizmoScale		-	Amount to scale/zoom uv coordinates
Offset			-	Offset to the pivot for the rotation effect
gizmoRot		-	Angle of rotation of uv
animate			-	Use speed values to alter uv coords
rotspeed		-	How fast uv will rotate when animate is on
speed			-	How fast uvs will translate when animate is on
spiral			-	Amount of spiral to add to the uv rotate
spirallim		-	Angular limit of spiral when Animate is on

****************************************
UV Tiles
****************************************
First test of a spritemanager style uv modifier, this can be used to alter the section of a texture displayed or to play animations. It is a wip so it may give odd results, for example the texture needs to be a power of 2.

Class:
public class UVTiles : Modifier
{
	public int		Frame;
	public int		TileWidth;
	public int		TileHeight;
	public Vector2	off;
	public Vector2	scale;
	public bool		Animate;
	public int		EndFrame;
	public float	fps;
	public bool		flipy;
	public bool		flipx;
}

Param Description:
Frame			-	Frame or tile to displat from sprite sheet
TileWidth		-	Width of the tiles on the sheet
TileHeight		-	Height of the tiles on the sheet
off				-	Adjust uv positions.
scale			-	Zoom uv coordinates
Animate			-	Play back the sprite sheet
EndFrame		-	Limit animation to first n tiles
fps				-	Desired playback speed
flipx			-	Flip uvs in the x direction
flipy			-	Flip uvs in the y direction

****************************************
WarpBind
****************************************
This modifier needs to be added to any object that you want to bind/connect to a world space warp ie ripple. You would first add a RippleWarp to an empty gamobject in the scene and set the desired params for that, then on the mesh object you want to be modified by that world space warp you would add ModifyObject as per normal and then add the WarpBind modifier selecting for the SourceWarpObj the game object with the ripple effect. The mesh will now deform based on the positions of the warp and the object. Both the world warp and the warpbind mod have decay values that work together to limit the range of the effect. The warp decay value will effect all objects bound to it whereas the WarpBind decay will only effect the object it is attached to.

Class:
public class WarpBind : Modifier
{
	public GameObject	SourceWarpObj;
	public float		decay;
}

Param Description:
SourceWarpObj	-	Space warp scene object to use for the deformation
decay			-	Limit the range of the deformation for this object

****************************************
Warp
****************************************
This is the base class for Space Warps, so most of these params will work across all space warps. Exceptions being ripple where it ignores the size and uses a radius value.

Class:
public class Warp : MonoBehaviour
{
	public float	Width;
	public float	Height;
	public float	Length;
	public float	Decay;
	public bool		Enabled;
	public bool		DisplayGizmo;
}

Param Description:
Width		-	Width of the warp
Height		-	Height of the warp
Length		-	Length(depth) of the warp
Decya		-	Decay value for the effect, this will effect all objects bound to the warp
Enabled		-	Is warp on
DisplaGizmo	-	Enable display of the gizmo in the editor

****************************************
Warps
****************************************
All the warps below behave and have the same params as their mesh based versions, so please refer to those of help;

The warps so far include are:
	Bend Warp
	Noise Warp
	Ripple Warp
	Skew Warp
	Stretch Warp
	Taper Warp
	Twist Warp
	Wave Warp

****************************************
PivotAdjust
****************************************
You can use this simple modifier to alter the pivot position of a mesh and or change its orientation. Use the normal Offset and gizmo params to adjust the pivot.

****************************************
Path Deform
****************************************
The PathDeform modifier deforms an object using a spline as a path. You can move and stretch the object along the path, and rotate and twist it about the path. The usual gizmo and offset values can also be used to alter the look of the deformation.

Class:
public class PathDeform : Modifier
{
	public float		percent;
	public float		stretch;
	public float		twist;
	public float		rotate;
	public Axis			axis;
	public bool			flip;
	public MegaShape	path;
	public bool			animate;
	public float		speed;
	public bool			drawpath;
	public float		tangent;
}

Param Description:
percent		-	Moves the object along the path based on a percentage of the path length.
stretch		-	Scales the object along the path, using the object's pivot point as the base of the scale.
twist		-	Twists the object about the path. The twist angle is based on the rotation of one end of the overall					length of the path. Typically, the deformed object takes up only a portion of the path, so the effect can				be subtle.
rotate		-	Rotates the object about the path.
axis		-	Changes the alignment of the path to local axis of the object
flip		-	Reverses the direction
path		-	Path to deform along, must be a MegaShape object in the scene
animate		-	Automatically move the object along the path.
speed		-	Speed to move the object
drawpath	-	Display the deformation path
tangent		-	How far to look ahead to get the direction of the path

****************************************
MegaShape
****************************************
MegaShape is a spline object from another system Iam working on. It allows the user to build shapes using knots and control the shape with handles on each knot. To use create an empty object in the scene and add the MegaShape script to it and then click add knot. Each knot can be positioned and its handles altered to get the desired shape. There is an option to close the shape as well. I am still working on the making the MegaShape easier to use and will be adding features to it in further releases.

****************************************
Enumerations:
****************************************
public enum MegaAxis
{
	X = 0,
	Y = 1,
	Z = 2,
};

public enum MegaEffectAxis
{
	X = 0,
	Y = 1,
	XY = 2,
};

public enum MegaMeltMat
{
	Ice = 0,
	Glass,
	Jelly,
	Plastic,
	Custom,
}

public enum MegaNormType
{
	Normals = 0,
	Vertices,
	Average,
}

public enum MegaChannel
{
	Red = 0,
	Green,
	Blue,
	Alpha,
}

****************************************
Changes:
****************************************
v1.00
Full rewrite of all core systems
Rewrite for Unity 2019.1 and newer
All modifiers converted to use Unity Burst and jobs.
New normal calculation system using Burst that is as fast as the Unity calc normals but preserves groups etc
Rewritten to use Burst and Jobs for between 5 to 50 times performance improvement
New auto disable system which will detect when a mesh does not need updating
Virtually no slow down when editing scenes with lots of modifiers
Seemless integration into Unity prefab system
Copying of objects now just works
Undo system fully functional
Animation window properly detects changes for easier animation of deformations
All workarounds for previous versions of Unity removed, streamlining the systems and improving performance
Wrap system updated and working with Burst for 5 times performance improvement
Path deform modifiers now uses Burst for spline interpolations gaining 20 times performance boost
Attractor modifier also makes use of Burst for finding nearest point on splines for 10 times boost in perfomance
Sprite deformation system, now you can stretch, squash, bend your Unity sprites.
2D FFD/Lattice modifiers specially for working with sprite
Full Text Mesh Pro support, both mesh and UI versions, updates with changes etc. Deform your UI text or 3d text.
Group deform support, deform multiple objects as one.
Support for deforming ProBuilder objects.
Improved Scroll making system, automatic setup.
Add your own modifiers.
Use low poly meshes to drive deformation of high poly ones
Works on all Graphic Pipelines
Works on Unity Platforms including VR and AR
Reorder modifier stack by dragging components in inspector
Proxy Mesh Collider option for faster collider updates
Improved worflow with simple dropdown box for adding modifiers
Quick edit area showing main params for all attached modifiers and quick disable/enable

Old features
50+ Deformers
Bezier Attractor
Bend
Bubble
Bulge
Collision Deform
Conform
Conform Multi
Crumple
Curve Deform
Curve Sculpt
Curve Scult Layered
Cylindrify
Deformable
Displace
Displace Limits
Displace Web Cam
Displace Render target
Dynamic Ripple
FFD 2x2x2
FFD 3x3x3
FFD 4x4x4
Globe
Hit Deform
Hump
Melt
Noise
Page Flip
Paint
Path Deform
Pivot Adjust
Point Cache
Push
Radial Skew
Relax
Ripple
Rolled
Rope Deform
Rubber
Scale
Simple Mod
Sinus Curve
Skew
Spherify
Squeeze
Stretch
Taper
Tree Bend
Twist
Vertex Anim
Vert Noise
Wave
Waving
World Path Deform

20+ Space Warps
Bend
Bubble
Cylindrify
FFD 2x2x2
FFD 3x3x3
FFD 4x4x4
Globe
Hump
Melt
Noise
Ripple
Sinus Curve
Skew
Spherify
Squeeze
Stretch
Taper
Twist
Wave
Waving
ZStretch Noise

Conform meshes to terrains or ground objects
Procedural box mesh
MegaShapes Lite Bezier spline system
Import animated splines directly from 3ds max
Make animated Splines inside Unity with our Spline Keyframe system
Turn splines into line meshes with spline to line/tubes or fill to mesh options
Spline exporters available for 3ds Max, Maya and Blender.
Kml, SVG, OSM spline import support
Wrap Mesh, wrap a mesh to a deforming mesh, great for clothing or facial hair etc
Animated Book
Attach objects to deforming mesh surfaces
Paper Scroll System
Bezier Patch image deform
Spline Path Follow System, objects, Rigid Bodies, Character Controller
Draw Spline at runtime system
Animated Hoses, connect two objects with deforming animating hose
Tracked Vehicle system
Train Follow system
Train Tracks system
Camera Orbit script

Follow me on Patreon for other Unity Assets and models https://www.patreon.com/ChrisWest

Not in.
Morph system, now integrates fully with the Unity BlendShapes system

Add later
warps we dont have bulge, crumple, curve deform, push, radial skew, scale, sinus curve, vert noise,

Version 1.56
Fixed again the FeeMoveHandle giving errors in Unity 2021.3

Version 1.55
Fixed a namespace issue in MegaGUI which would appear if you used AssemblyDefs.
Fixed wrong version of Unity for FreeMoveHandle rot removal resulting in obsolete method warning in 2022
Fixed wrong version of Unity for RayCastCommand in the conform modifier.

Version 1.54
Added a help box to the Wrap inspector to say that the wrap target needs at least one modifier applied. This stops an error if you hit the map button when the target has no deformations added.
Added a Sprite Deform example scene
Updating a MegaSprite will now update the sprite texture as well.
If you click Update Mesh in MegaSprite the current deformation will now be displayed after the click instead of resetting and then displaying when a change is made.

Version 1.53
Probuilder support is off by default as it is no longer a default installed package. If you need Probuilder support search for 'Probuilder' in the scripts an replace the #if false with #if true
Updated the dependency so latest Burst is used and the old jobs package is not required.

Version 1.52
Fixed an error in the MegaSprite component when it is added to a Sprite object.
To use the Sprite system, add MegaSprite and then click the 'Update Sprite' button. A child mesh object will be added which is where you add the deformations.
Added a help box to MegaSprite to tell you to click the Update Sprite button if required.
Fixed an issue with dragging the handles on the 2D FFD gizmos only moving the bottom left corner. Works correctly now.

Version 1.51
Fixed an error in the editor script for the Multi Conform modifier

Version 1.50
Fixed an issue in  the MegaRubber editor script which stopped you changing the size value.
Added an option to the FFD Controller script to disable the physics calcs, useful if you want to control the FFD with a Rigidbody and springs for example.

Version 1.49
Fixed an issue when Reloading a scene with Modifiers in it in the Editor would stop modifiers from being updated.

Version 1.48
Small fix for an obsolete method warning on some versions of Unity 2021

Version 1.47
Conform Modifier now has a slider in the quick edit for the comform amount
Conform Multi Modifier now has a slider in the quick edit for the comform amount
Stopped the error in Conform Modifier when there is no target selected
Added ApplyModsToGroup() method to MegaModifyObject, this will allow the making of modify groups at runtime. First call AddToGroup(MegaModifyObject modobj) to add objects to the group then call ApplyModsToGroup()

Version 1.46
Fix errors that some versions of Unity 2021.3 get regarding missing methods.
Adding a prefab to a scene should automatically update the mesh now, which should fix an issues of seeing no meshes when adding a prefab with deformed meshes.

Version 1.45
Conform modifier has been fully converted to use Burst and Jobs, so now Raycasts are done in a job massively reducing the CPU usage for this modifier.
Removed the odd lag in the conform modifiers when a modifier above them in the stack was changing
Added most used Conform modifier params to the Quick Params section for easier editing.

Version 1.44
MegaFiers 2 made fully compatible with Unity 2023
Updated the Conform Modifier so it works correctly if it is not the first or only modifier on the stack
Updated the Conform Multi Modifier so it works correctly if it is not the first or only modifier on the stack

Version 1.43
Fixed a small bug with the dynamic ripple modifier where if selected and you exit play mode it gives a reference exception error.

Version 1.42
Fixed AnimationCurves not working with Undo system.

Version 1.41
Fixed a bug in the Rubber modifier which could cause an exception in some cases.

Version 1.40
Small update to the hose system, will work correctly now if parent object or end node is moved or rotated and the hose is in the same hierarchy.

Version 1.39
Fixed bug in the Hit Deform modifier where a memory array was not being reset causing mesh to go wrong when adding the modifier.

Version 1.38
Fixed an issue with the Rubber modifier that stopped it from working.

Version 1.37
Fixed a bug in the Wrap system which would leave points un mapped to the target mesh.
Added an option to the Wrap system to set the resolution of the Voxel creation, lower values will be slower mapping, higher values are quicker.
The max dist value will effect the voxels used in the mapping, keep it as low as the approz distance of the furthes distance a point on the wrapping mesh is away from the target, increase if mapping does not find all points.
The lower the max dist value the faster the mapping will be, with the right values of Voxel Res and max distance mapping should be very fast now.

Version 1.36
Small change to the Paint modifier as by default it would need the 'Always Changing' box to be checked to see the deformations, now works without.
Added Beta feature of being able to use a texture as a mask for where deformation happens on a mesh.
Fixed Bone weights being reset when Play mode entered
Added ',' to Bone Selection search to show all bones with non zero weights

Version 1.35
Fixed a rare bug on some VR platforms where Unity could lose the normals on a mesh in a build. Put a check in for that to automatically rebuild normals if Unity decides to lose them.
Added a beta feature to allow you to select which part of a skinned mesh is deformed by using vertex bone weights. You can set per bone how much any vertex using that bone is deformed.
The inspector for Deformable Skin now has a section showing the bone selection weights.
There is a search option to show the bones you want, type in part of a bone name ie spine and only those bones will shown
If you put '.' in the search it will only show the bones whose weights are not 1
A Set Value option added so you set that value to any bones showing.

Version 1.34
MegaFiers updated to support latest versions of Burst and Jobs
Poke modifier updated to fix an issue with the Latest Jobs version not liking empty arrays
Waving modifier updated to fix an issue with the Latest Jobs version not liking empty arrays

Version 1.33
Added a curve option to the Waving modifier so you can control where on the mesh the waving happens, useful for trees.

Version 1.32
Added Support for Deformable Skinned objects to get correct deformations even when animating and vertices are deformed a long way from the rest pose
Added a new Poke Modifier allowing you to interact with objects as if they were soft, great for Skin, punches, soft toys, pillows, beds and so on.

Version 1.31
Fixed a bug in the Melt modifier when used with a group.
Melt Modifier sped up a little.

Version 1.30
Fixed some issues in the Spline Auto Smooth system where expecially on open splines the end point handles could be wrong.

Version 1.29
Fixed an issue with the Point Cache Modifier not working if No Jobs is selected or building for WebGL. 
Removed the DisplaceWebcam modifier which could cause issues on Mac platforms.
Fixed an error if you have a single modifier on a mesh which is using Vertex Colors to limit the deformation and you then disable the modifier.

Version 1.28
Fixed an issue with the Mirroring of moving FFD control points.
Mirroring the editing of FFD control points now works correctly for multi selected control points, before only the first selected FFD control points movements were being mirrored.

Version 1.27
Fixed an issue with the FFD modifiers where the control points could drift while editing due to floating point errors.

Version 1.26
Added a simple demo scene for the Scroll System.
Unity has an issue saving Blendshapes created directly in Unity, so added a new system to fix that problem so Blendshapes created by MegaFiers are now kept nicely.

Version 1.25
Fixed a bug in the Conform Modifier if use Local Down set to false.

Version 1.24
Fixed an issue when a MegaSprite object was saved to a prefab and its texture would be missing when added back to a scene.
Fixed an issue when there were multiple MegaSprite prefabs added to a scene that textures would change.

Version 1.23
MegaFiers 2 made compatible with Unity 2021.1
Removed obsolete warning in Unity 2021.1
Changes made to BlendShape Workshop system so mesh now correctly marked as dirty when creating/deleting blendshapes from modifiers so Blendshapes will be saved correctly.
MegaFiers compatible with Burst 1.5.4
MegaFiers compatible with Jobs 0.8.0
Removed various Packages that somehow got included as dependencies.

Version 1.22
Added a Mesh Combiner system which can combine multiple mesh objects with multiple materials, just select the parent object and go to the MegaFiers menu and select Combine for a new object to created with all the meshes combined.

Version 1.21
Fixed an issue which could cause an error if building the demo scene to a player.

Version 1.20
Changes to the Sprite deforming system to preserve the layer sorting info so the deforming sprite mesh renders correctly with respect to other sprites.

Version 1.19
Fix to the Attach system when attaching to a Skinned mesh, works correctly now.

Version 1.18
MegaFiers now works correctly with meshes that require 32bit triangle indices ie have over 65535 vertices.

Version 1.17
Updated the MegaWarpBind inspector so it will only show Warp objects when you change the warp value
Changed the AutoDisable system for Warps to take into account the moving, rotation and scaling of warp objects.
Added Warp Object to the Quick Edit section of the Warp Bind Modifier
Added Decay value to the Warp Bind quick edit section

Version 1.16
Added a MegaFFDMap utility component to allow you to easily made any GameObject to a FFD lattice point so you can create your own FFD Controller systems easily.

Version 1.15
Added FFDController example script to get softbody type effects using a FFD3x3x3 modifier.
Added FFDControlAxis example script to get softbody type effects using a FFD3x3x3 modifier, as an axis value to say which axis of the lattice the deformation happens on.

Version 1.14
Changed Unity Collections ReadOnly attribute to use full namespace to avoid conflicts with assets that have their own ReadOnly attribute.

Version 1.13
Added option to FFD modifiers to show either Index or XYZ values
Fixed an issue with FFD lattice handles sometimes causing an error

Version 1.12
Added GetPointWorld and SetPointWorld methods to the FFD modifiers so you can easily control the Lattice points from external scripts.

Version 1.11
Updated World Path Deform and Path Deform to be a bit faster as well as to stop the error if JobsDebugger was enabled

Version 1.10
Fixed a problem that stopped builds from compiling.

Version 1.09
Improvements to the Point Cache system
Point Cache performance has been increased due to new data layout and optimizing playback functions
Reduced memory used by Point Cache modifier with new data layout
Improved the Show Mapping options for Point Cache with addition of show every nth vertex for when dealing with high res meshes
Added option to Point Cache to manually set Offset, Rotation and Scaling or imported cache data making it easier to get Cache data with odd transformations mapping correctly
Point Cache will now only update if param such as time is changed
Point Cache data now stored in a Scriptable object
Point Cache Inspector much faster now data has been moved to a Scriptable Object
Point Cache will now auto disable if no changes are happening

Version 1.08
Point Cache system changed to stop Native Array errors, new version of Point Cache being worked on for future update.

Version 1.07
Fixed an unneeded alloc in Path deforms if stertch or twist curves are not used
Fixed issue in MegaBakeMeshTest which would cause an error when building

Version 1.06
You can now bake the current deformed mesh to a simple prefab.
Fixed not being able to change Amp value in Waving modifier
Fixed not being able to change Size value in Rubber Modifier
MegaFiers menu has option to sub divide a mesh so you can increase the vertex count for an object before deforming it
Add a MegaBakeMeshTest script that will bake a deforming mesh to a new objects with the deformations baked into Blendshapes, showing how you could add 1000's of variations of an object to a scene all GPU instanced (in pipelines that support instanced Skinned Meshes)

Version 1.05
Fixed an issue with FFD modifiers where the lattice handles were offset

Version 1.04
Fixed an error that could happen when displaying Wrap Debug gizmos
Added a warning to the Wrap system if trying to wrap a skinned mesh to a skinned mesh
Added option in the above case to convert the wrapping mesh to a non skinned mesh
Added option to disable max dist check on wrapping map check

Version 1.03
You can now add the deformations you create as BlendShapes to the object so allowing you to make complex deformations and save those out for much faster animation. You can add frames, remove them, add multiple BlendShapes etc

Version 1.02
Noise modifier optimized so only evaluate if Strength value is non zero
Fixed Shrink and Gap values changing not triggering an update to Wrapped mesh
Wrap now works correctly with Skinned Meshes
Fixed an issue with Wrap not working with Skinned meshes that had just blendshapes and no bones
Added system to Wrap to keep track of Blendshape weight changes so Wrap will only update when it needs to

Version 1.01
Fixed Issue in Group system for some modifiers if objects were different sizes
Fixed Exception if Attach systems Modify object is deleted.
Fixed Group system adding Particle system objects as meshes when using Add All Child Meshes button
Added Gantry Object to Asset
Added Can now Disable an Object in the group from being deformed.
Added Can now Toggle Group deformation
Fixed Issue with Gizmo size being wrong in a group.
Fixed Issue stopping Undos updating meshes on prefab objects
Fixed Modifiers not working on Text Mesh Pro UI objects until Text Changed
Fixed Wrong calculation in the Burst/Job AnimationCurve evaluator
Improved Auto Disable system
Fixed Out of range evaluations on Burst Animation curve
Added Option to use vertex Colors to control deformation amount
Added a Weight value so you blend between no deformation and full deformation
Added option to disable Jobs
Wrap system now has option to get bounds info from target mesh resulting in a nice speed up
Wrap system now has local space option which allows wrapped object to be moved relative to the target
Change Wrap so it you can now update a complex mesh with a complex stack of modifiers applied to a simpler version of the mesh
Wrap system can now auto disable if target mesh is not changing
Big speed up in the Wrapping setup code
Added Attach System for Wrap objects
Attach system streamlined making it easier to use
Option added to Modify Object to Automatically attach any children to the deforming mesh
Option added to the Wrap system to Automatically attach any children to the wrapped mesh

Version 1.0
Original Release

workflow improvements
Add Modify Object
Add modifiers from drop down
reorder modifiers
remove and disable
quick edit area
copy objects
make prefabs
group deform
attach children
wrap
collider update
