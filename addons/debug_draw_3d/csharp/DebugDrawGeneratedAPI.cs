/// 1.2.0
/// ////////////////////////////////////////////////
/// THIS FILE HAS BEEN GENERATED.
/// THE CHANGES IN THIS FILE WILL BE OVERWRITTEN
/// AFTER THE UPDATE OR AFTER THE RESTART!
/// ////////////////////////////////////////////////

using Godot;
using System;

static internal class DebugDraw2D
{
    private static GodotObject _instance;
    public static GodotObject Instance
    {
        get
        {
            if (!GodotObject.IsInstanceValid(_instance))
            {
                _instance = Engine.GetSingleton("DebugDraw2D");
            }
            return _instance;
        }
    }
    
    private static readonly StringName __clear_all = "clear_all";
    private static readonly StringName __begin_text_group = "begin_text_group";
    private static readonly StringName __end_text_group = "end_text_group";
    private static readonly StringName __set_text = "set_text";
    private static readonly StringName __create_graph = "create_graph";
    private static readonly StringName __create_fps_graph = "create_fps_graph";
    private static readonly StringName __graph_update_data = "graph_update_data";
    private static readonly StringName __remove_graph = "remove_graph";
    private static readonly StringName __clear_graphs = "clear_graphs";
    private static readonly StringName __get_graph = "get_graph";
    private static readonly StringName __get_graph_names = "get_graph_names";
    private static readonly StringName __get_render_stats = "get_render_stats";
    
    public static void ClearAll()
    {
#if !DEBUG && !FORCED_DD3D
        if (_DebugDrawUtils_.IsCallEnabled)
#endif
        {
#if (!DEBUG || FORCED_DD3D) || (DEBUG && !FORCED_DD3D)
            Instance?.Call(__clear_all);
#endif
        }
    }
    
    public static void BeginTextGroup(string group_title, int group_priority = 0, Color? group_color = null, bool show_title = true, int title_size = -1, int text_size = -1)
    {
#if !DEBUG && !FORCED_DD3D
        if (_DebugDrawUtils_.IsCallEnabled)
#endif
        {
#if (!DEBUG || FORCED_DD3D) || (DEBUG && !FORCED_DD3D)
            Instance?.Call(__begin_text_group, group_title, group_priority, group_color ?? _DebugDrawUtils_.DefaultArgumentsData.arg_0, show_title, title_size, text_size);
#endif
        }
    }
    
    public static void EndTextGroup()
    {
#if !DEBUG && !FORCED_DD3D
        if (_DebugDrawUtils_.IsCallEnabled)
#endif
        {
#if (!DEBUG || FORCED_DD3D) || (DEBUG && !FORCED_DD3D)
            Instance?.Call(__end_text_group);
#endif
        }
    }
    
    public static void SetText(string key, Variant? value = null, int priority = 0, Color? color_of_value = null, float duration = -1f)
    {
#if !DEBUG && !FORCED_DD3D
        if (_DebugDrawUtils_.IsCallEnabled)
#endif
        {
#if (!DEBUG || FORCED_DD3D) || (DEBUG && !FORCED_DD3D)
            Instance?.Call(__set_text, key, value ?? _DebugDrawUtils_.DefaultArgumentsData.arg_1, priority, color_of_value ?? _DebugDrawUtils_.DefaultArgumentsData.arg_0, duration);
#endif
        }
    }
    
    public static DebugDrawGraph CreateGraph(StringName title)
    {
#if !DEBUG && !FORCED_DD3D
        if (_DebugDrawUtils_.IsCallEnabled)
#endif
        {
#if (!DEBUG || FORCED_DD3D) || (DEBUG && !FORCED_DD3D)
            return (DebugDrawGraph)_DebugDrawUtils_.CreateWrapperFromObject((GodotObject)Instance?.Call(__create_graph, title));
#endif
        }
#if !DEBUG && !FORCED_DD3D
        else
#endif
        {
#if !DEBUG && !FORCED_DD3D
            return default;
#endif
        }
    }
    
    public static DebugDrawGraph CreateFpsGraph(StringName title)
    {
#if !DEBUG && !FORCED_DD3D
        if (_DebugDrawUtils_.IsCallEnabled)
#endif
        {
#if (!DEBUG || FORCED_DD3D) || (DEBUG && !FORCED_DD3D)
            return (DebugDrawGraph)_DebugDrawUtils_.CreateWrapperFromObject((GodotObject)Instance?.Call(__create_fps_graph, title));
#endif
        }
#if !DEBUG && !FORCED_DD3D
        else
#endif
        {
#if !DEBUG && !FORCED_DD3D
            return default;
#endif
        }
    }
    
    public static void GraphUpdateData(StringName title, float data)
    {
#if !DEBUG && !FORCED_DD3D
        if (_DebugDrawUtils_.IsCallEnabled)
#endif
        {
#if (!DEBUG || FORCED_DD3D) || (DEBUG && !FORCED_DD3D)
            Instance?.Call(__graph_update_data, title, data);
#endif
        }
    }
    
    public static void RemoveGraph(StringName title)
    {
#if !DEBUG && !FORCED_DD3D
        if (_DebugDrawUtils_.IsCallEnabled)
#endif
        {
#if (!DEBUG || FORCED_DD3D) || (DEBUG && !FORCED_DD3D)
            Instance?.Call(__remove_graph, title);
#endif
        }
    }
    
    public static void ClearGraphs()
    {
#if !DEBUG && !FORCED_DD3D
        if (_DebugDrawUtils_.IsCallEnabled)
#endif
        {
#if (!DEBUG || FORCED_DD3D) || (DEBUG && !FORCED_DD3D)
            Instance?.Call(__clear_graphs);
#endif
        }
    }
    
    public static DebugDrawGraph GetGraph(StringName title)
    {
#if !DEBUG && !FORCED_DD3D
        if (_DebugDrawUtils_.IsCallEnabled)
#endif
        {
#if (!DEBUG || FORCED_DD3D) || (DEBUG && !FORCED_DD3D)
            return (DebugDrawGraph)_DebugDrawUtils_.CreateWrapperFromObject((GodotObject)Instance?.Call(__get_graph, title));
#endif
        }
#if !DEBUG && !FORCED_DD3D
        else
#endif
        {
#if !DEBUG && !FORCED_DD3D
            return default;
#endif
        }
    }
    
    public static string[] GetGraphNames()
    {
#if !DEBUG && !FORCED_DD3D
        if (_DebugDrawUtils_.IsCallEnabled)
#endif
        {
#if (!DEBUG || FORCED_DD3D) || (DEBUG && !FORCED_DD3D)
            return (string[])(Instance?.Call(__get_graph_names));
#endif
        }
#if !DEBUG && !FORCED_DD3D
        else
#endif
        {
#if !DEBUG && !FORCED_DD3D
            return default;
#endif
        }
    }
    
    public static DebugDrawStats2D GetRenderStats()
    {
#if !DEBUG && !FORCED_DD3D
        if (_DebugDrawUtils_.IsCallEnabled)
#endif
        {
#if (!DEBUG || FORCED_DD3D) || (DEBUG && !FORCED_DD3D)
            return (DebugDrawStats2D)_DebugDrawUtils_.CreateWrapperFromObject((GodotObject)Instance?.Call(__get_render_stats));
#endif
        }
#if !DEBUG && !FORCED_DD3D
        else
#endif
        {
#if !DEBUG && !FORCED_DD3D
            return default;
#endif
        }
    }
    
    private static readonly StringName __prop_empty_color = "empty_color";
    private static readonly StringName __prop_debug_enabled = "debug_enabled";
    private static readonly StringName __prop_config = "config";
    private static readonly StringName __prop_custom_canvas = "custom_canvas";
    
    public static Color EmptyColor
    {
        get => (Color)ClassDB.ClassGetProperty(Instance, __prop_empty_color);
        set => ClassDB.ClassSetProperty(Instance, __prop_empty_color, value);
    }
    
    public static bool DebugEnabled
    {
        get => (bool)ClassDB.ClassGetProperty(Instance, __prop_debug_enabled);
        set => ClassDB.ClassSetProperty(Instance, __prop_debug_enabled, value);
    }
    
    public static DebugDrawConfig2D Config
    {
        get => new DebugDrawConfig2D((GodotObject)ClassDB.ClassGetProperty(Instance, __prop_config));
        set => ClassDB.ClassSetProperty(Instance, __prop_config, value.Instance);
    }
    
    public static Control CustomCanvas
    {
        get => (Control)ClassDB.ClassGetProperty(Instance, __prop_custom_canvas);
        set => ClassDB.ClassSetProperty(Instance, __prop_custom_canvas, value);
    }
    
}

internal class DebugDrawStats2D : IDisposable
{
    public GodotObject Instance { get; private set; }
    public DebugDrawStats2D(GodotObject _instance)
    {
        if (_instance == null) throw new ArgumentNullException("_instance");
        if (!ClassDB.IsParentClass(_instance.GetClass(), GetType().Name)) throw new ArgumentException("\"_instance\" has the wrong type.");
        Instance = _instance;
    }
    
    public void Dispose()
    {
        Instance.Dispose();
        Instance = null;
    }
    
    public DebugDrawStats2D() : this((GodotObject)ClassDB.Instantiate("DebugDrawStats2D")) { }
    
    private static readonly StringName __prop_overlay_text_groups = "overlay_text_groups";
    private static readonly StringName __prop_overlay_text_lines = "overlay_text_lines";
    private static readonly StringName __prop_overlay_graphs_enabled = "overlay_graphs_enabled";
    private static readonly StringName __prop_overlay_graphs_total = "overlay_graphs_total";
    
    public int OverlayTextGroups
    {
        get => (int)ClassDB.ClassGetProperty(Instance, __prop_overlay_text_groups);
        set => ClassDB.ClassSetProperty(Instance, __prop_overlay_text_groups, value);
    }
    
    public int OverlayTextLines
    {
        get => (int)ClassDB.ClassGetProperty(Instance, __prop_overlay_text_lines);
        set => ClassDB.ClassSetProperty(Instance, __prop_overlay_text_lines, value);
    }
    
    public int OverlayGraphsEnabled
    {
        get => (int)ClassDB.ClassGetProperty(Instance, __prop_overlay_graphs_enabled);
        set => ClassDB.ClassSetProperty(Instance, __prop_overlay_graphs_enabled, value);
    }
    
    public int OverlayGraphsTotal
    {
        get => (int)ClassDB.ClassGetProperty(Instance, __prop_overlay_graphs_total);
        set => ClassDB.ClassSetProperty(Instance, __prop_overlay_graphs_total, value);
    }
    
}

internal class DebugDrawConfig2D : IDisposable
{
    public GodotObject Instance { get; private set; }
    public DebugDrawConfig2D(GodotObject _instance)
    {
        if (_instance == null) throw new ArgumentNullException("_instance");
        if (!ClassDB.IsParentClass(_instance.GetClass(), GetType().Name)) throw new ArgumentException("\"_instance\" has the wrong type.");
        Instance = _instance;
    }
    
    public void Dispose()
    {
        Instance.Dispose();
        Instance = null;
    }
    
    public DebugDrawConfig2D() : this((GodotObject)ClassDB.Instantiate("DebugDrawConfig2D")) { }
    
    public enum BlockPosition : long
    {
        LeftTop = 0,
        RightTop = 1,
        LeftBottom = 2,
        RightBottom = 3,
    }
    
    private static readonly StringName __prop_graphs_base_offset = "graphs_base_offset";
    private static readonly StringName __prop_text_block_position = "text_block_position";
    private static readonly StringName __prop_text_block_offset = "text_block_offset";
    private static readonly StringName __prop_text_padding = "text_padding";
    private static readonly StringName __prop_text_default_duration = "text_default_duration";
    private static readonly StringName __prop_text_default_size = "text_default_size";
    private static readonly StringName __prop_text_foreground_color = "text_foreground_color";
    private static readonly StringName __prop_text_background_color = "text_background_color";
    private static readonly StringName __prop_text_custom_font = "text_custom_font";
    
    public Vector2I GraphsBaseOffset
    {
        get => (Vector2I)ClassDB.ClassGetProperty(Instance, __prop_graphs_base_offset);
        set => ClassDB.ClassSetProperty(Instance, __prop_graphs_base_offset, value);
    }
    
    public DebugDrawConfig2D.BlockPosition TextBlockPosition
    {
        get => (DebugDrawConfig2D.BlockPosition)(long)ClassDB.ClassGetProperty(Instance, __prop_text_block_position);
        set => ClassDB.ClassSetProperty(Instance, __prop_text_block_position, (long)value);
    }
    
    public Vector2I TextBlockOffset
    {
        get => (Vector2I)ClassDB.ClassGetProperty(Instance, __prop_text_block_offset);
        set => ClassDB.ClassSetProperty(Instance, __prop_text_block_offset, value);
    }
    
    public Vector2I TextPadding
    {
        get => (Vector2I)ClassDB.ClassGetProperty(Instance, __prop_text_padding);
        set => ClassDB.ClassSetProperty(Instance, __prop_text_padding, value);
    }
    
    public float TextDefaultDuration
    {
        get => (float)ClassDB.ClassGetProperty(Instance, __prop_text_default_duration);
        set => ClassDB.ClassSetProperty(Instance, __prop_text_default_duration, value);
    }
    
    public int TextDefaultSize
    {
        get => (int)ClassDB.ClassGetProperty(Instance, __prop_text_default_size);
        set => ClassDB.ClassSetProperty(Instance, __prop_text_default_size, value);
    }
    
    public Color TextForegroundColor
    {
        get => (Color)ClassDB.ClassGetProperty(Instance, __prop_text_foreground_color);
        set => ClassDB.ClassSetProperty(Instance, __prop_text_foreground_color, value);
    }
    
    public Color TextBackgroundColor
    {
        get => (Color)ClassDB.ClassGetProperty(Instance, __prop_text_background_color);
        set => ClassDB.ClassSetProperty(Instance, __prop_text_background_color, value);
    }
    
    public Font TextCustomFont
    {
        get => (Font)ClassDB.ClassGetProperty(Instance, __prop_text_custom_font);
        set => ClassDB.ClassSetProperty(Instance, __prop_text_custom_font, value);
    }
    
}

internal class DebugDrawGraph : IDisposable
{
    public GodotObject Instance { get; private set; }
    public DebugDrawGraph(GodotObject _instance)
    {
        if (_instance == null) throw new ArgumentNullException("_instance");
        if (!ClassDB.IsParentClass(_instance.GetClass(), GetType().Name)) throw new ArgumentException("\"_instance\" has the wrong type.");
        Instance = _instance;
    }
    
    public void Dispose()
    {
        Instance.Dispose();
        Instance = null;
    }
    
    public DebugDrawGraph() : this((GodotObject)ClassDB.Instantiate("DebugDrawGraph")) { }
    
    public enum GraphPosition : long
    {
        LeftTop = 0,
        RightTop = 1,
        LeftBottom = 2,
        RightBottom = 3,
    }
    
    public enum GraphSide : long
    {
        Left = 0,
        Top = 1,
        Right = 2,
        Bottom = 3,
    }
    
    public enum TextFlags : long
    {
        Current = 1,
        Avg = 2,
        Max = 4,
        Min = 8,
        All = 15,
    }
    
    private static readonly StringName __get_title = "get_title";
    private static readonly StringName __set_parent = "set_parent";
    
    public StringName GetTitle()
    {
#if !DEBUG && !FORCED_DD3D
        if (_DebugDrawUtils_.IsCallEnabled)
#endif
        {
#if (!DEBUG || FORCED_DD3D) || (DEBUG && !FORCED_DD3D)
            return (StringName)(Instance?.Call(__get_title));
#endif
        }
#if !DEBUG && !FORCED_DD3D
        else
#endif
        {
#if !DEBUG && !FORCED_DD3D
            return default;
#endif
        }
    }
    
    public void SetParent(StringName parent, DebugDrawGraph.GraphSide side = (DebugDrawGraph.GraphSide)3)
    {
#if !DEBUG && !FORCED_DD3D
        if (_DebugDrawUtils_.IsCallEnabled)
#endif
        {
#if (!DEBUG || FORCED_DD3D) || (DEBUG && !FORCED_DD3D)
            Instance?.Call(__set_parent, parent, (long)side);
#endif
        }
    }
    
    private static readonly StringName __prop_enabled = "enabled";
    private static readonly StringName __prop_upside_down = "upside_down";
    private static readonly StringName __prop_show_title = "show_title";
    private static readonly StringName __prop_show_text_flags = "show_text_flags";
    private static readonly StringName __prop_size = "size";
    private static readonly StringName __prop_buffer_size = "buffer_size";
    private static readonly StringName __prop_offset = "offset";
    private static readonly StringName __prop_corner = "corner";
    private static readonly StringName __prop_line_width = "line_width";
    private static readonly StringName __prop_line_color = "line_color";
    private static readonly StringName __prop_background_color = "background_color";
    private static readonly StringName __prop_border_color = "border_color";
    private static readonly StringName __prop_text_suffix = "text_suffix";
    private static readonly StringName __prop_custom_font = "custom_font";
    private static readonly StringName __prop_title_size = "title_size";
    private static readonly StringName __prop_text_size = "text_size";
    private static readonly StringName __prop_title_color = "title_color";
    private static readonly StringName __prop_text_color = "text_color";
    private static readonly StringName __prop_text_precision = "text_precision";
    private static readonly StringName __prop_parent_graph = "parent_graph";
    private static readonly StringName __prop_parent_graph_side = "parent_graph_side";
    private static readonly StringName __prop_data_getter = "data_getter";
    
    public bool Enabled
    {
        get => (bool)ClassDB.ClassGetProperty(Instance, __prop_enabled);
        set => ClassDB.ClassSetProperty(Instance, __prop_enabled, value);
    }
    
    public bool UpsideDown
    {
        get => (bool)ClassDB.ClassGetProperty(Instance, __prop_upside_down);
        set => ClassDB.ClassSetProperty(Instance, __prop_upside_down, value);
    }
    
    public bool ShowTitle
    {
        get => (bool)ClassDB.ClassGetProperty(Instance, __prop_show_title);
        set => ClassDB.ClassSetProperty(Instance, __prop_show_title, value);
    }
    
    public DebugDrawGraph.TextFlags ShowTextFlags
    {
        get => (DebugDrawGraph.TextFlags)(long)ClassDB.ClassGetProperty(Instance, __prop_show_text_flags);
        set => ClassDB.ClassSetProperty(Instance, __prop_show_text_flags, (long)value);
    }
    
    public Vector2I Size
    {
        get => (Vector2I)ClassDB.ClassGetProperty(Instance, __prop_size);
        set => ClassDB.ClassSetProperty(Instance, __prop_size, value);
    }
    
    public int BufferSize
    {
        get => (int)ClassDB.ClassGetProperty(Instance, __prop_buffer_size);
        set => ClassDB.ClassSetProperty(Instance, __prop_buffer_size, value);
    }
    
    public Vector2I Offset
    {
        get => (Vector2I)ClassDB.ClassGetProperty(Instance, __prop_offset);
        set => ClassDB.ClassSetProperty(Instance, __prop_offset, value);
    }
    
    public DebugDrawGraph.GraphPosition Corner
    {
        get => (DebugDrawGraph.GraphPosition)(long)ClassDB.ClassGetProperty(Instance, __prop_corner);
        set => ClassDB.ClassSetProperty(Instance, __prop_corner, (long)value);
    }
    
    public float LineWidth
    {
        get => (float)ClassDB.ClassGetProperty(Instance, __prop_line_width);
        set => ClassDB.ClassSetProperty(Instance, __prop_line_width, value);
    }
    
    public Color LineColor
    {
        get => (Color)ClassDB.ClassGetProperty(Instance, __prop_line_color);
        set => ClassDB.ClassSetProperty(Instance, __prop_line_color, value);
    }
    
    public Color BackgroundColor
    {
        get => (Color)ClassDB.ClassGetProperty(Instance, __prop_background_color);
        set => ClassDB.ClassSetProperty(Instance, __prop_background_color, value);
    }
    
    public Color BorderColor
    {
        get => (Color)ClassDB.ClassGetProperty(Instance, __prop_border_color);
        set => ClassDB.ClassSetProperty(Instance, __prop_border_color, value);
    }
    
    public string TextSuffix
    {
        get => (string)ClassDB.ClassGetProperty(Instance, __prop_text_suffix);
        set => ClassDB.ClassSetProperty(Instance, __prop_text_suffix, value);
    }
    
    public Font CustomFont
    {
        get => (Font)ClassDB.ClassGetProperty(Instance, __prop_custom_font);
        set => ClassDB.ClassSetProperty(Instance, __prop_custom_font, value);
    }
    
    public int TitleSize
    {
        get => (int)ClassDB.ClassGetProperty(Instance, __prop_title_size);
        set => ClassDB.ClassSetProperty(Instance, __prop_title_size, value);
    }
    
    public int TextSize
    {
        get => (int)ClassDB.ClassGetProperty(Instance, __prop_text_size);
        set => ClassDB.ClassSetProperty(Instance, __prop_text_size, value);
    }
    
    public Color TitleColor
    {
        get => (Color)ClassDB.ClassGetProperty(Instance, __prop_title_color);
        set => ClassDB.ClassSetProperty(Instance, __prop_title_color, value);
    }
    
    public Color TextColor
    {
        get => (Color)ClassDB.ClassGetProperty(Instance, __prop_text_color);
        set => ClassDB.ClassSetProperty(Instance, __prop_text_color, value);
    }
    
    public int TextPrecision
    {
        get => (int)ClassDB.ClassGetProperty(Instance, __prop_text_precision);
        set => ClassDB.ClassSetProperty(Instance, __prop_text_precision, value);
    }
    
    public StringName ParentGraph
    {
        get => (StringName)ClassDB.ClassGetProperty(Instance, __prop_parent_graph);
        set => ClassDB.ClassSetProperty(Instance, __prop_parent_graph, value);
    }
    
    public DebugDrawGraph.GraphSide ParentGraphSide
    {
        get => (DebugDrawGraph.GraphSide)(long)ClassDB.ClassGetProperty(Instance, __prop_parent_graph_side);
        set => ClassDB.ClassSetProperty(Instance, __prop_parent_graph_side, (long)value);
    }
    
    public Callable DataGetter
    {
        get => (Callable)ClassDB.ClassGetProperty(Instance, __prop_data_getter);
        set => ClassDB.ClassSetProperty(Instance, __prop_data_getter, value);
    }
    
}

internal class DebugDrawFPSGraph : DebugDrawGraph
{
    
    public DebugDrawFPSGraph(GodotObject _instance) : base (_instance) {}
    
    public DebugDrawFPSGraph() : this((GodotObject)ClassDB.Instantiate("DebugDrawFPSGraph")) { }
    
    private static readonly StringName __prop_frame_time_mode = "frame_time_mode";
    
    public bool FrameTimeMode
    {
        get => (bool)ClassDB.ClassGetProperty(Instance, __prop_frame_time_mode);
        set => ClassDB.ClassSetProperty(Instance, __prop_frame_time_mode, value);
    }
    
}

static internal class DebugDraw3D
{
    private static GodotObject _instance;
    public static GodotObject Instance
    {
        get
        {
            if (!GodotObject.IsInstanceValid(_instance))
            {
                _instance = Engine.GetSingleton("DebugDraw3D");
            }
            return _instance;
        }
    }
    
    private static readonly StringName __clear_all = "clear_all";
    private static readonly StringName __draw_sphere = "draw_sphere";
    private static readonly StringName __draw_sphere_xf = "draw_sphere_xf";
    private static readonly StringName __draw_sphere_hd = "draw_sphere_hd";
    private static readonly StringName __draw_sphere_hd_xf = "draw_sphere_hd_xf";
    private static readonly StringName __draw_cylinder = "draw_cylinder";
    private static readonly StringName __draw_box = "draw_box";
    private static readonly StringName __draw_box_xf = "draw_box_xf";
    private static readonly StringName __draw_aabb = "draw_aabb";
    private static readonly StringName __draw_aabb_ab = "draw_aabb_ab";
    private static readonly StringName __draw_line_hit = "draw_line_hit";
    private static readonly StringName __draw_line_hit_offset = "draw_line_hit_offset";
    private static readonly StringName __draw_line = "draw_line";
    private static readonly StringName __draw_lines = "draw_lines";
    private static readonly StringName __draw_ray = "draw_ray";
    private static readonly StringName __draw_line_path = "draw_line_path";
    private static readonly StringName __draw_arrow = "draw_arrow";
    private static readonly StringName __draw_arrow_line = "draw_arrow_line";
    private static readonly StringName __draw_arrow_ray = "draw_arrow_ray";
    private static readonly StringName __draw_arrow_path = "draw_arrow_path";
    private static readonly StringName __draw_point_path = "draw_point_path";
    private static readonly StringName __draw_square = "draw_square";
    private static readonly StringName __draw_points = "draw_points";
    private static readonly StringName __draw_camera_frustum = "draw_camera_frustum";
    private static readonly StringName __draw_camera_frustum_planes = "draw_camera_frustum_planes";
    private static readonly StringName __draw_position = "draw_position";
    private static readonly StringName __draw_gizmo = "draw_gizmo";
    private static readonly StringName __draw_grid = "draw_grid";
    private static readonly StringName __draw_grid_xf = "draw_grid_xf";
    private static readonly StringName __get_render_stats = "get_render_stats";
    
    public static void ClearAll()
    {
#if !DEBUG && !FORCED_DD3D
        if (_DebugDrawUtils_.IsCallEnabled)
#endif
        {
#if (!DEBUG || FORCED_DD3D) || (DEBUG && !FORCED_DD3D)
            Instance?.Call(__clear_all);
#endif
        }
    }
    
    public static void DrawSphere(Vector3 position, float radius = 0.5f, Color? color = null, float duration = 0f)
    {
#if !DEBUG && !FORCED_DD3D
        if (_DebugDrawUtils_.IsCallEnabled)
#endif
        {
#if (!DEBUG || FORCED_DD3D) || (DEBUG && !FORCED_DD3D)
            Instance?.Call(__draw_sphere, position, radius, color ?? _DebugDrawUtils_.DefaultArgumentsData.arg_0, duration);
#endif
        }
    }
    
    public static void DrawSphereXf(Transform3D transform, Color? color = null, float duration = 0f)
    {
#if !DEBUG && !FORCED_DD3D
        if (_DebugDrawUtils_.IsCallEnabled)
#endif
        {
#if (!DEBUG || FORCED_DD3D) || (DEBUG && !FORCED_DD3D)
            Instance?.Call(__draw_sphere_xf, transform, color ?? _DebugDrawUtils_.DefaultArgumentsData.arg_0, duration);
#endif
        }
    }
    
    public static void DrawSphereHd(Vector3 position, float radius = 0.5f, Color? color = null, float duration = 0f)
    {
#if !DEBUG && !FORCED_DD3D
        if (_DebugDrawUtils_.IsCallEnabled)
#endif
        {
#if (!DEBUG || FORCED_DD3D) || (DEBUG && !FORCED_DD3D)
            Instance?.Call(__draw_sphere_hd, position, radius, color ?? _DebugDrawUtils_.DefaultArgumentsData.arg_0, duration);
#endif
        }
    }
    
    public static void DrawSphereHdXf(Transform3D transform, Color? color = null, float duration = 0f)
    {
#if !DEBUG && !FORCED_DD3D
        if (_DebugDrawUtils_.IsCallEnabled)
#endif
        {
#if (!DEBUG || FORCED_DD3D) || (DEBUG && !FORCED_DD3D)
            Instance?.Call(__draw_sphere_hd_xf, transform, color ?? _DebugDrawUtils_.DefaultArgumentsData.arg_0, duration);
#endif
        }
    }
    
    public static void DrawCylinder(Transform3D transform, Color? color = null, float duration = 0f)
    {
#if !DEBUG && !FORCED_DD3D
        if (_DebugDrawUtils_.IsCallEnabled)
#endif
        {
#if (!DEBUG || FORCED_DD3D) || (DEBUG && !FORCED_DD3D)
            Instance?.Call(__draw_cylinder, transform, color ?? _DebugDrawUtils_.DefaultArgumentsData.arg_0, duration);
#endif
        }
    }
    
    public static void DrawBox(Vector3 position, Vector3 size, Color? color = null, bool is_box_centered = false, float duration = 0f)
    {
#if !DEBUG && !FORCED_DD3D
        if (_DebugDrawUtils_.IsCallEnabled)
#endif
        {
#if (!DEBUG || FORCED_DD3D) || (DEBUG && !FORCED_DD3D)
            Instance?.Call(__draw_box, position, size, color ?? _DebugDrawUtils_.DefaultArgumentsData.arg_0, is_box_centered, duration);
#endif
        }
    }
    
    public static void DrawBoxXf(Transform3D transform, Color? color = null, bool is_box_centered = true, float duration = 0f)
    {
#if !DEBUG && !FORCED_DD3D
        if (_DebugDrawUtils_.IsCallEnabled)
#endif
        {
#if (!DEBUG || FORCED_DD3D) || (DEBUG && !FORCED_DD3D)
            Instance?.Call(__draw_box_xf, transform, color ?? _DebugDrawUtils_.DefaultArgumentsData.arg_0, is_box_centered, duration);
#endif
        }
    }
    
    public static void DrawAabb(Aabb aabb, Color? color = null, float duration = 0f)
    {
#if !DEBUG && !FORCED_DD3D
        if (_DebugDrawUtils_.IsCallEnabled)
#endif
        {
#if (!DEBUG || FORCED_DD3D) || (DEBUG && !FORCED_DD3D)
            Instance?.Call(__draw_aabb, aabb, color ?? _DebugDrawUtils_.DefaultArgumentsData.arg_0, duration);
#endif
        }
    }
    
    public static void DrawAabbAb(Vector3 a, Vector3 b, Color? color = null, float duration = 0f)
    {
#if !DEBUG && !FORCED_DD3D
        if (_DebugDrawUtils_.IsCallEnabled)
#endif
        {
#if (!DEBUG || FORCED_DD3D) || (DEBUG && !FORCED_DD3D)
            Instance?.Call(__draw_aabb_ab, a, b, color ?? _DebugDrawUtils_.DefaultArgumentsData.arg_0, duration);
#endif
        }
    }
    
    public static void DrawLineHit(Vector3 start, Vector3 end, Vector3 hit, bool is_hit, float hit_size = 0.25f, Color? hit_color = null, Color? after_hit_color = null, float duration = 0f)
    {
#if !DEBUG && !FORCED_DD3D
        if (_DebugDrawUtils_.IsCallEnabled)
#endif
        {
#if (!DEBUG || FORCED_DD3D) || (DEBUG && !FORCED_DD3D)
            Instance?.Call(__draw_line_hit, start, end, hit, is_hit, hit_size, hit_color ?? _DebugDrawUtils_.DefaultArgumentsData.arg_0, after_hit_color ?? _DebugDrawUtils_.DefaultArgumentsData.arg_0, duration);
#endif
        }
    }
    
    public static void DrawLineHitOffset(Vector3 start, Vector3 end, bool is_hit, float unit_offset_of_hit = 0.5f, float hit_size = 0.25f, Color? hit_color = null, Color? after_hit_color = null, float duration = 0f)
    {
#if !DEBUG && !FORCED_DD3D
        if (_DebugDrawUtils_.IsCallEnabled)
#endif
        {
#if (!DEBUG || FORCED_DD3D) || (DEBUG && !FORCED_DD3D)
            Instance?.Call(__draw_line_hit_offset, start, end, is_hit, unit_offset_of_hit, hit_size, hit_color ?? _DebugDrawUtils_.DefaultArgumentsData.arg_0, after_hit_color ?? _DebugDrawUtils_.DefaultArgumentsData.arg_0, duration);
#endif
        }
    }
    
    public static void DrawLine(Vector3 a, Vector3 b, Color? color = null, float duration = 0f)
    {
#if !DEBUG && !FORCED_DD3D
        if (_DebugDrawUtils_.IsCallEnabled)
#endif
        {
#if (!DEBUG || FORCED_DD3D) || (DEBUG && !FORCED_DD3D)
            Instance?.Call(__draw_line, a, b, color ?? _DebugDrawUtils_.DefaultArgumentsData.arg_0, duration);
#endif
        }
    }
    
    public static void DrawLines(Vector3[] lines, Color? color = null, float duration = 0f)
    {
#if !DEBUG && !FORCED_DD3D
        if (_DebugDrawUtils_.IsCallEnabled)
#endif
        {
#if (!DEBUG || FORCED_DD3D) || (DEBUG && !FORCED_DD3D)
            Instance?.Call(__draw_lines, lines, color ?? _DebugDrawUtils_.DefaultArgumentsData.arg_0, duration);
#endif
        }
    }
    
    public static void DrawRay(Vector3 origin, Vector3 direction, float length, Color? color = null, float duration = 0f)
    {
#if !DEBUG && !FORCED_DD3D
        if (_DebugDrawUtils_.IsCallEnabled)
#endif
        {
#if (!DEBUG || FORCED_DD3D) || (DEBUG && !FORCED_DD3D)
            Instance?.Call(__draw_ray, origin, direction, length, color ?? _DebugDrawUtils_.DefaultArgumentsData.arg_0, duration);
#endif
        }
    }
    
    public static void DrawLinePath(Vector3[] path, Color? color = null, float duration = 0f)
    {
#if !DEBUG && !FORCED_DD3D
        if (_DebugDrawUtils_.IsCallEnabled)
#endif
        {
#if (!DEBUG || FORCED_DD3D) || (DEBUG && !FORCED_DD3D)
            Instance?.Call(__draw_line_path, path, color ?? _DebugDrawUtils_.DefaultArgumentsData.arg_0, duration);
#endif
        }
    }
    
    public static void DrawArrow(Transform3D transform, Color? color = null, float duration = 0f)
    {
#if !DEBUG && !FORCED_DD3D
        if (_DebugDrawUtils_.IsCallEnabled)
#endif
        {
#if (!DEBUG || FORCED_DD3D) || (DEBUG && !FORCED_DD3D)
            Instance?.Call(__draw_arrow, transform, color ?? _DebugDrawUtils_.DefaultArgumentsData.arg_0, duration);
#endif
        }
    }
    
    public static void DrawArrowLine(Vector3 a, Vector3 b, Color? color = null, float arrow_size = 0.5f, bool is_absolute_size = false, float duration = 0f)
    {
#if !DEBUG && !FORCED_DD3D
        if (_DebugDrawUtils_.IsCallEnabled)
#endif
        {
#if (!DEBUG || FORCED_DD3D) || (DEBUG && !FORCED_DD3D)
            Instance?.Call(__draw_arrow_line, a, b, color ?? _DebugDrawUtils_.DefaultArgumentsData.arg_0, arrow_size, is_absolute_size, duration);
#endif
        }
    }
    
    public static void DrawArrowRay(Vector3 origin, Vector3 direction, float length, Color? color = null, float arrow_size = 0.5f, bool is_absolute_size = false, float duration = 0f)
    {
#if !DEBUG && !FORCED_DD3D
        if (_DebugDrawUtils_.IsCallEnabled)
#endif
        {
#if (!DEBUG || FORCED_DD3D) || (DEBUG && !FORCED_DD3D)
            Instance?.Call(__draw_arrow_ray, origin, direction, length, color ?? _DebugDrawUtils_.DefaultArgumentsData.arg_0, arrow_size, is_absolute_size, duration);
#endif
        }
    }
    
    public static void DrawArrowPath(Vector3[] path, Color? color = null, float arrow_size = 0.75f, bool is_absolute_size = true, float duration = 0f)
    {
#if !DEBUG && !FORCED_DD3D
        if (_DebugDrawUtils_.IsCallEnabled)
#endif
        {
#if (!DEBUG || FORCED_DD3D) || (DEBUG && !FORCED_DD3D)
            Instance?.Call(__draw_arrow_path, path, color ?? _DebugDrawUtils_.DefaultArgumentsData.arg_0, arrow_size, is_absolute_size, duration);
#endif
        }
    }
    
    public static void DrawPointPath(Vector3[] path, float size = 0.25f, Color? points_color = null, Color? lines_color = null, float duration = 0f)
    {
#if !DEBUG && !FORCED_DD3D
        if (_DebugDrawUtils_.IsCallEnabled)
#endif
        {
#if (!DEBUG || FORCED_DD3D) || (DEBUG && !FORCED_DD3D)
            Instance?.Call(__draw_point_path, path, size, points_color ?? _DebugDrawUtils_.DefaultArgumentsData.arg_0, lines_color ?? _DebugDrawUtils_.DefaultArgumentsData.arg_0, duration);
#endif
        }
    }
    
    public static void DrawSquare(Vector3 position, float size = 0.20000000298023f, Color? color = null, float duration = 0f)
    {
#if !DEBUG && !FORCED_DD3D
        if (_DebugDrawUtils_.IsCallEnabled)
#endif
        {
#if (!DEBUG || FORCED_DD3D) || (DEBUG && !FORCED_DD3D)
            Instance?.Call(__draw_square, position, size, color ?? _DebugDrawUtils_.DefaultArgumentsData.arg_0, duration);
#endif
        }
    }
    
    public static void DrawPoints(Vector3[] points, float size = 0.20000000298023f, Color? color = null, float duration = 0f)
    {
#if !DEBUG && !FORCED_DD3D
        if (_DebugDrawUtils_.IsCallEnabled)
#endif
        {
#if (!DEBUG || FORCED_DD3D) || (DEBUG && !FORCED_DD3D)
            Instance?.Call(__draw_points, points, size, color ?? _DebugDrawUtils_.DefaultArgumentsData.arg_0, duration);
#endif
        }
    }
    
    public static void DrawCameraFrustum(Camera3D camera, Color? color = null, float duration = 0f)
    {
#if !DEBUG && !FORCED_DD3D
        if (_DebugDrawUtils_.IsCallEnabled)
#endif
        {
#if (!DEBUG || FORCED_DD3D) || (DEBUG && !FORCED_DD3D)
            Instance?.Call(__draw_camera_frustum, camera, color ?? _DebugDrawUtils_.DefaultArgumentsData.arg_0, duration);
#endif
        }
    }
    
    public static void DrawCameraFrustumPlanes(Godot.Collections.Array camera_frustum, Color? color = null, float duration = 0f)
    {
#if !DEBUG && !FORCED_DD3D
        if (_DebugDrawUtils_.IsCallEnabled)
#endif
        {
#if (!DEBUG || FORCED_DD3D) || (DEBUG && !FORCED_DD3D)
            Instance?.Call(__draw_camera_frustum_planes, camera_frustum, color ?? _DebugDrawUtils_.DefaultArgumentsData.arg_0, duration);
#endif
        }
    }
    
    public static void DrawPosition(Transform3D transform, Color? color = null, float duration = 0f)
    {
#if !DEBUG && !FORCED_DD3D
        if (_DebugDrawUtils_.IsCallEnabled)
#endif
        {
#if (!DEBUG || FORCED_DD3D) || (DEBUG && !FORCED_DD3D)
            Instance?.Call(__draw_position, transform, color ?? _DebugDrawUtils_.DefaultArgumentsData.arg_0, duration);
#endif
        }
    }
    
    public static void DrawGizmo(Transform3D transform, Color? color = null, bool is_centered = false, float duration = 0f)
    {
#if !DEBUG && !FORCED_DD3D
        if (_DebugDrawUtils_.IsCallEnabled)
#endif
        {
#if (!DEBUG || FORCED_DD3D) || (DEBUG && !FORCED_DD3D)
            Instance?.Call(__draw_gizmo, transform, color ?? _DebugDrawUtils_.DefaultArgumentsData.arg_0, is_centered, duration);
#endif
        }
    }
    
    public static void DrawGrid(Vector3 origin, Vector3 x_size, Vector3 y_size, Vector2I subdivision, Color? color = null, bool is_centered = true, float duration = 0f)
    {
#if !DEBUG && !FORCED_DD3D
        if (_DebugDrawUtils_.IsCallEnabled)
#endif
        {
#if (!DEBUG || FORCED_DD3D) || (DEBUG && !FORCED_DD3D)
            Instance?.Call(__draw_grid, origin, x_size, y_size, subdivision, color ?? _DebugDrawUtils_.DefaultArgumentsData.arg_0, is_centered, duration);
#endif
        }
    }
    
    public static void DrawGridXf(Transform3D transform, Vector2I subdivision, Color? color = null, bool is_centered = true, float duration = 0f)
    {
#if !DEBUG && !FORCED_DD3D
        if (_DebugDrawUtils_.IsCallEnabled)
#endif
        {
#if (!DEBUG || FORCED_DD3D) || (DEBUG && !FORCED_DD3D)
            Instance?.Call(__draw_grid_xf, transform, subdivision, color ?? _DebugDrawUtils_.DefaultArgumentsData.arg_0, is_centered, duration);
#endif
        }
    }
    
    public static DebugDrawStats3D GetRenderStats()
    {
#if !DEBUG && !FORCED_DD3D
        if (_DebugDrawUtils_.IsCallEnabled)
#endif
        {
#if (!DEBUG || FORCED_DD3D) || (DEBUG && !FORCED_DD3D)
            return (DebugDrawStats3D)_DebugDrawUtils_.CreateWrapperFromObject((GodotObject)Instance?.Call(__get_render_stats));
#endif
        }
#if !DEBUG && !FORCED_DD3D
        else
#endif
        {
#if !DEBUG && !FORCED_DD3D
            return default;
#endif
        }
    }
    
    private static readonly StringName __prop_empty_color = "empty_color";
    private static readonly StringName __prop_debug_enabled = "debug_enabled";
    private static readonly StringName __prop_config = "config";
    private static readonly StringName __prop_custom_viewport = "custom_viewport";
    
    public static Color EmptyColor
    {
        get => (Color)ClassDB.ClassGetProperty(Instance, __prop_empty_color);
        set => ClassDB.ClassSetProperty(Instance, __prop_empty_color, value);
    }
    
    public static bool DebugEnabled
    {
        get => (bool)ClassDB.ClassGetProperty(Instance, __prop_debug_enabled);
        set => ClassDB.ClassSetProperty(Instance, __prop_debug_enabled, value);
    }
    
    public static DebugDrawConfig3D Config
    {
        get => new DebugDrawConfig3D((GodotObject)ClassDB.ClassGetProperty(Instance, __prop_config));
        set => ClassDB.ClassSetProperty(Instance, __prop_config, value.Instance);
    }
    
    public static Viewport CustomViewport
    {
        get => (Viewport)ClassDB.ClassGetProperty(Instance, __prop_custom_viewport);
        set => ClassDB.ClassSetProperty(Instance, __prop_custom_viewport, value);
    }
    
}

internal class DebugDrawStats3D : IDisposable
{
    public GodotObject Instance { get; private set; }
    public DebugDrawStats3D(GodotObject _instance)
    {
        if (_instance == null) throw new ArgumentNullException("_instance");
        if (!ClassDB.IsParentClass(_instance.GetClass(), GetType().Name)) throw new ArgumentException("\"_instance\" has the wrong type.");
        Instance = _instance;
    }
    
    public void Dispose()
    {
        Instance.Dispose();
        Instance = null;
    }
    
    public DebugDrawStats3D() : this((GodotObject)ClassDB.Instantiate("DebugDrawStats3D")) { }
    
    private static readonly StringName __prop_instances = "instances";
    private static readonly StringName __prop_lines = "lines";
    private static readonly StringName __prop_total_geometry = "total_geometry";
    private static readonly StringName __prop_visible_instances = "visible_instances";
    private static readonly StringName __prop_visible_lines = "visible_lines";
    private static readonly StringName __prop_total_visible = "total_visible";
    private static readonly StringName __prop_time_filling_buffers_instances_usec = "time_filling_buffers_instances_usec";
    private static readonly StringName __prop_time_filling_buffers_lines_usec = "time_filling_buffers_lines_usec";
    private static readonly StringName __prop_total_time_filling_buffers_usec = "total_time_filling_buffers_usec";
    private static readonly StringName __prop_time_culling_instant_usec = "time_culling_instant_usec";
    private static readonly StringName __prop_time_culling_delayed_usec = "time_culling_delayed_usec";
    private static readonly StringName __prop_total_time_culling_usec = "total_time_culling_usec";
    private static readonly StringName __prop_total_time_spent_usec = "total_time_spent_usec";
    
    public int Instances
    {
        get => (int)ClassDB.ClassGetProperty(Instance, __prop_instances);
        set => ClassDB.ClassSetProperty(Instance, __prop_instances, value);
    }
    
    public int Lines
    {
        get => (int)ClassDB.ClassGetProperty(Instance, __prop_lines);
        set => ClassDB.ClassSetProperty(Instance, __prop_lines, value);
    }
    
    public int TotalGeometry
    {
        get => (int)ClassDB.ClassGetProperty(Instance, __prop_total_geometry);
        set => ClassDB.ClassSetProperty(Instance, __prop_total_geometry, value);
    }
    
    public int VisibleInstances
    {
        get => (int)ClassDB.ClassGetProperty(Instance, __prop_visible_instances);
        set => ClassDB.ClassSetProperty(Instance, __prop_visible_instances, value);
    }
    
    public int VisibleLines
    {
        get => (int)ClassDB.ClassGetProperty(Instance, __prop_visible_lines);
        set => ClassDB.ClassSetProperty(Instance, __prop_visible_lines, value);
    }
    
    public int TotalVisible
    {
        get => (int)ClassDB.ClassGetProperty(Instance, __prop_total_visible);
        set => ClassDB.ClassSetProperty(Instance, __prop_total_visible, value);
    }
    
    public int TimeFillingBuffersInstancesUsec
    {
        get => (int)ClassDB.ClassGetProperty(Instance, __prop_time_filling_buffers_instances_usec);
        set => ClassDB.ClassSetProperty(Instance, __prop_time_filling_buffers_instances_usec, value);
    }
    
    public int TimeFillingBuffersLinesUsec
    {
        get => (int)ClassDB.ClassGetProperty(Instance, __prop_time_filling_buffers_lines_usec);
        set => ClassDB.ClassSetProperty(Instance, __prop_time_filling_buffers_lines_usec, value);
    }
    
    public int TotalTimeFillingBuffersUsec
    {
        get => (int)ClassDB.ClassGetProperty(Instance, __prop_total_time_filling_buffers_usec);
        set => ClassDB.ClassSetProperty(Instance, __prop_total_time_filling_buffers_usec, value);
    }
    
    public int TimeCullingInstantUsec
    {
        get => (int)ClassDB.ClassGetProperty(Instance, __prop_time_culling_instant_usec);
        set => ClassDB.ClassSetProperty(Instance, __prop_time_culling_instant_usec, value);
    }
    
    public int TimeCullingDelayedUsec
    {
        get => (int)ClassDB.ClassGetProperty(Instance, __prop_time_culling_delayed_usec);
        set => ClassDB.ClassSetProperty(Instance, __prop_time_culling_delayed_usec, value);
    }
    
    public int TotalTimeCullingUsec
    {
        get => (int)ClassDB.ClassGetProperty(Instance, __prop_total_time_culling_usec);
        set => ClassDB.ClassSetProperty(Instance, __prop_total_time_culling_usec, value);
    }
    
    public int TotalTimeSpentUsec
    {
        get => (int)ClassDB.ClassGetProperty(Instance, __prop_total_time_spent_usec);
        set => ClassDB.ClassSetProperty(Instance, __prop_total_time_spent_usec, value);
    }
    
}

internal class DebugDrawConfig3D : IDisposable
{
    public GodotObject Instance { get; private set; }
    public DebugDrawConfig3D(GodotObject _instance)
    {
        if (_instance == null) throw new ArgumentNullException("_instance");
        if (!ClassDB.IsParentClass(_instance.GetClass(), GetType().Name)) throw new ArgumentException("\"_instance\" has the wrong type.");
        Instance = _instance;
    }
    
    public void Dispose()
    {
        Instance.Dispose();
        Instance = null;
    }
    
    public DebugDrawConfig3D() : this((GodotObject)ClassDB.Instantiate("DebugDrawConfig3D")) { }
    
    private static readonly StringName __prop_freeze_3d_render = "freeze_3d_render";
    private static readonly StringName __prop_visible_instance_bounds = "visible_instance_bounds";
    private static readonly StringName __prop_use_frustum_culling = "use_frustum_culling";
    private static readonly StringName __prop_cull_by_distance = "cull_by_distance";
    private static readonly StringName __prop_force_use_camera_from_scene = "force_use_camera_from_scene";
    private static readonly StringName __prop_geometry_render_layers = "geometry_render_layers";
    private static readonly StringName __prop_line_hit_color = "line_hit_color";
    private static readonly StringName __prop_line_after_hit_color = "line_after_hit_color";
    
    public bool Freeze3dRender
    {
        get => (bool)ClassDB.ClassGetProperty(Instance, __prop_freeze_3d_render);
        set => ClassDB.ClassSetProperty(Instance, __prop_freeze_3d_render, value);
    }
    
    public bool VisibleInstanceBounds
    {
        get => (bool)ClassDB.ClassGetProperty(Instance, __prop_visible_instance_bounds);
        set => ClassDB.ClassSetProperty(Instance, __prop_visible_instance_bounds, value);
    }
    
    public bool UseFrustumCulling
    {
        get => (bool)ClassDB.ClassGetProperty(Instance, __prop_use_frustum_culling);
        set => ClassDB.ClassSetProperty(Instance, __prop_use_frustum_culling, value);
    }
    
    public float CullByDistance
    {
        get => (float)ClassDB.ClassGetProperty(Instance, __prop_cull_by_distance);
        set => ClassDB.ClassSetProperty(Instance, __prop_cull_by_distance, value);
    }
    
    public bool ForceUseCameraFromScene
    {
        get => (bool)ClassDB.ClassGetProperty(Instance, __prop_force_use_camera_from_scene);
        set => ClassDB.ClassSetProperty(Instance, __prop_force_use_camera_from_scene, value);
    }
    
    public int GeometryRenderLayers
    {
        get => (int)ClassDB.ClassGetProperty(Instance, __prop_geometry_render_layers);
        set => ClassDB.ClassSetProperty(Instance, __prop_geometry_render_layers, value);
    }
    
    public Color LineHitColor
    {
        get => (Color)ClassDB.ClassGetProperty(Instance, __prop_line_hit_color);
        set => ClassDB.ClassSetProperty(Instance, __prop_line_hit_color, value);
    }
    
    public Color LineAfterHitColor
    {
        get => (Color)ClassDB.ClassGetProperty(Instance, __prop_line_after_hit_color);
        set => ClassDB.ClassSetProperty(Instance, __prop_line_after_hit_color, value);
    }
    
}

static internal class DebugDrawManager
{
    private static GodotObject _instance;
    public static GodotObject Instance
    {
        get
        {
            if (!GodotObject.IsInstanceValid(_instance))
            {
                _instance = Engine.GetSingleton("DebugDrawManager");
            }
            return _instance;
        }
    }
    
    private static readonly StringName __clear_all = "clear_all";
    
    public static void ClearAll()
    {
#if !DEBUG && !FORCED_DD3D
        if (_DebugDrawUtils_.IsCallEnabled)
#endif
        {
#if (!DEBUG || FORCED_DD3D) || (DEBUG && !FORCED_DD3D)
            Instance?.Call(__clear_all);
#endif
        }
    }
    
    private static readonly StringName __prop_debug_enabled = "debug_enabled";
    
    public static bool DebugEnabled
    {
        get => (bool)ClassDB.ClassGetProperty(Instance, __prop_debug_enabled);
        set => ClassDB.ClassSetProperty(Instance, __prop_debug_enabled, value);
    }
    
}

internal static class _DebugDrawUtils_
{
    const bool is_debug_enabled =
#if DEBUG
    true;
#else
    false;
#endif
    public static readonly bool IsCallEnabled = is_debug_enabled || OS.HasFeature("forced_dd3d");
    
    public static class DefaultArgumentsData
    {
        public static readonly Color arg_0 = new Color(0f, 0f, 0f, 0f);
        public static readonly Variant arg_1 = default;
    }
    
    public static object CreateWrapperFromObject(GodotObject _instance)
    {
        if (_instance == null)
        {
            return null;
        }
        switch(_instance.GetClass())
        {
            case "DebugDrawStats2D":
            {
                return new DebugDrawStats2D(_instance);
            }
            case "DebugDrawConfig2D":
            {
                return new DebugDrawConfig2D(_instance);
            }
            case "DebugDrawGraph":
            {
                return new DebugDrawGraph(_instance);
            }
            case "DebugDrawFPSGraph":
            {
                return new DebugDrawFPSGraph(_instance);
            }
            case "DebugDrawStats3D":
            {
                return new DebugDrawStats3D(_instance);
            }
            case "DebugDrawConfig3D":
            {
                return new DebugDrawConfig3D(_instance);
            }
        }
        throw new NotImplementedException();
    }
}
