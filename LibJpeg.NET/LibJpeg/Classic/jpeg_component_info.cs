﻿/* Copyright (C) 2008-2009, Bit Miracle
 * http://www.bitmiracle.com
 * 
 * Copyright (C) 1994-1996, Thomas G. Lane.
 * This file is part of the Independent JPEG Group's software.
 * For conditions of distribution and use, see the accompanying README file.
 *
 */

using System;
using System.Collections.Generic;
using System.Text;

using BitMiracle.LibJpeg.Classic.Internal;

namespace BitMiracle.LibJpeg.Classic
{
    /// <summary>
    /// Basic info about one component (color channel).
    /// </summary>
    public class jpeg_component_info
    {
        /* These values are fixed over the whole image. */
        /* For compression, they must be supplied by parameter setup; */
        /* for decompression, they are read from the SOF marker. */
        public int component_id = 0;       /* identifier for this component (0..255) */
        public int component_index = 0;        /* its index in SOF or cinfo.comp_info[] */
        public int h_samp_factor = 0;      /* horizontal sampling factor (1..4) */
        public int v_samp_factor = 0;      /* vertical sampling factor (1..4) */
        public int quant_tbl_no = 0;       /* quantization table selector (0..3) */
        /* These values may vary between scans. */
        /* For compression, they must be supplied by parameter setup; */
        /* for decompression, they are read from the SOS marker. */
        /* The decompressor output side may not use these variables. */
        public int dc_tbl_no = 0;      /* DC entropy table selector (0..3) */
        public int ac_tbl_no = 0;      /* AC entropy table selector (0..3) */

        /* Remaining fields should be treated as private by applications. */

        /* These values are computed during compression or decompression startup: */
        /* Component's size in DCT blocks.
         * Any dummy blocks added to complete an MCU are not counted; therefore
         * these values do not depend on whether a scan is interleaved or not.
         */
        public int width_in_blocks = 0;
        internal int height_in_blocks = 0;
        /* Size of a DCT block in samples.  Always DCTSIZE for compression.
         * For decompression this is the size of the output from one DCT block,
         * reflecting any scaling we choose to apply during the IDCT step.
         * Values of 1,2,4,8 are likely to be supported.  Note that different
         * components may receive different IDCT scalings.
         */
        internal int DCT_scaled_size = 0;
        /* The downsampled dimensions are the component's actual, unpadded number
         * of samples at the main buffer (preprocessing/compression interface), thus
         * downsampled_width = ceil(image_width * Hi/Hmax)
         * and similarly for height.  For decompression, IDCT scaling is included, so
         * downsampled_width = ceil(image_width * Hi/Hmax * DCT_scaled_size/DCTSIZE)
         */
        internal int downsampled_width = 0;    /* actual width in samples */
	
        internal int downsampled_height = 0; /* actual height in samples */
        /* This flag is used only for decompression.  In cases where some of the
         * components will be ignored (eg grayscale output from YCbCr image),
         * we can skip most computations for the unused components.
         */
        internal bool component_needed;  /* do we need the value of this component? */

        /* These values are computed before starting a scan of the component. */
        /* The decompressor output side may not use these variables. */
        internal int MCU_width = 0;      /* number of blocks per MCU, horizontally */
        internal int MCU_height = 0;     /* number of blocks per MCU, vertically */
        internal int MCU_blocks = 0;     /* MCU_width * MCU_height */
        internal int MCU_sample_width = 0;       /* MCU width in samples, MCU_width*DCT_scaled_size */
        internal int last_col_width = 0;     /* # of non-dummy blocks across in last MCU */
        internal int last_row_height = 0;        /* # of non-dummy blocks down in last MCU */

        /* Saved quantization table for component; null if none yet saved.
         * See jpeg_input_controller comments about the need for this information.
         * This field is currently used only for decompression.
         */
        internal JQUANT_TBL quant_table = null;

        internal jpeg_component_info()
        {
        }

        internal void Assign(jpeg_component_info ci)
        {
            component_id = ci.component_id;
            component_index = ci.component_index;
            h_samp_factor = ci.h_samp_factor;
            v_samp_factor = ci.v_samp_factor;
            quant_tbl_no = ci.quant_tbl_no;
            dc_tbl_no = ci.dc_tbl_no;
            ac_tbl_no = ci.ac_tbl_no;
            width_in_blocks = ci.width_in_blocks;
            height_in_blocks = ci.height_in_blocks;
            DCT_scaled_size = ci.DCT_scaled_size;
            downsampled_width = ci.downsampled_width;
            downsampled_height = ci.downsampled_height;
            component_needed = ci.component_needed;
            MCU_width = ci.MCU_width;
            MCU_height = ci.MCU_height;
            MCU_blocks = ci.MCU_blocks;
            MCU_sample_width = ci.MCU_sample_width;
            last_col_width = ci.last_col_width;
            last_row_height = ci.last_row_height;
            quant_table = ci.quant_table;
        }

        public int Downsampled_width
	    {
		    get { return downsampled_width; }
	    }

        internal static jpeg_component_info[] createArrayOfComponents(int length)
        {
            if (length < 0)
                throw new ArgumentOutOfRangeException("length");

            jpeg_component_info[] result = new jpeg_component_info[length];
            for (int i = 0; i < result.Length; ++i)
                result[i] = new jpeg_component_info();

            return result;
        }
    }
}