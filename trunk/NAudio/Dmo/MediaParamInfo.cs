﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace NAudio.Dmo
{
    /// <summary>
    /// MP_PARAMINFO
    /// </summary>
    struct MediaParamInfo
    {
        MediaParamType mpType;
        MediaParamCurveType mopCaps;
        float mpdMinValue; // MP_DATA is a float
        float mpdMaxValue;
        float mpdNeutralValue;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]        
        string szUnitText;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        string szLabel; 
    }

    /// <summary>
    /// MP_TYPE
    /// </summary>
    enum MediaParamType 
    {
        /// <summary>
        /// MPT_INT
        /// </summary>
        Int,
        /// <summary>
        /// MPT_FLOAT
        /// </summary>
        Float,
        /// <summary>
        /// MPT_BOOL
        /// </summary>
        Bool,
        /// <summary>
        /// MPT_ENUM
        /// </summary>
        Enum,
        /// <summary>
        /// MPT_MAX
        /// </summary>
        Max,
    }
    /// <summary>
    /// MP_CURVE_TYPE
    /// </summary>
    [Flags]
    enum MediaParamCurveType
    {	
        MP_CURVE_JUMP	= 0x1,
	    MP_CURVE_LINEAR	= 0x2,
	    MP_CURVE_SQUARE	= 0x4,
	    MP_CURVE_INVSQUARE = 0x8,
	    MP_CURVE_SINE	= 0x10
    }

}
