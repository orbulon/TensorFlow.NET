﻿using System;
using System.Collections.Generic;
using System.Text;
using static Tensorflow.Binding;

namespace Tensorflow.Train
{
    public class moving_averages
    {
        /// <summary>
        /// Compute the moving average of a variable.
        /// </summary>
        /// <param name="variable"></param>
        /// <param name="value"></param>
        /// <param name="decay"></param>
        /// <param name="zero_debias"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Tensor assign_moving_average(RefVariable variable, RefVariable value, Tensor decay,
            bool zero_debias = true, string name = null)
        {
            tf_with(ops.name_scope(name, "", new { variable, value, decay }), scope =>
            {
                decay = ops.convert_to_tensor(1.0f - decay, name: "decay");
                if (decay.dtype != variable.dtype.as_base_dtype())
                    decay = math_ops.cast(decay, variable.dtype.as_base_dtype());
            });

            throw new NotImplementedException("assign_moving_average");
        }
    }
}