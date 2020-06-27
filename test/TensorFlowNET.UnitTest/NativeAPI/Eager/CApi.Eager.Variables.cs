﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Tensorflow;
using static Tensorflow.Binding;

namespace TensorFlowNET.UnitTest.NativeAPI
{
    public partial class CApiEagerTest
    {
        /// <summary>
        /// TEST(CAPI, Variables)
        /// </summary>
        [TestMethod]
        public unsafe void Variables()
        {
            using var status = c_api.TF_NewStatus();
            var opts = TFE_NewContextOptions();
            var ctx = TFE_NewContext(opts, status);
            ASSERT_EQ(TF_OK, TF_GetCode(status), TF_Message(status));
            TFE_DeleteContextOptions(opts);

            var var_handle = CreateVariable(ctx, 12.0f, status);
            ASSERT_EQ(TF_OK, TF_GetCode(status), TF_Message(status));

            var op = TFE_NewOp(ctx, "ReadVariableOp", status);
            ASSERT_EQ(TF_OK, TF_GetCode(status), TF_Message(status));
            TFE_OpSetAttrType(op, "dtype", TF_FLOAT);
            TFE_OpAddInput(op, var_handle, status);
            ASSERT_EQ(TF_OK, TF_GetCode(status), TF_Message(status));
            int num_retvals = 1;
            var value_handle = new[] { IntPtr.Zero };
            TFE_Execute(op, value_handle, ref num_retvals, status);
            TFE_DeleteOp(op);

            ASSERT_EQ(TF_OK, TF_GetCode(status), TF_Message(status));
            ASSERT_EQ(1, num_retvals);
            EXPECT_EQ(TF_FLOAT, TFE_TensorHandleDataType(value_handle[0]));
            EXPECT_EQ(0, TFE_TensorHandleNumDims(value_handle[0], status));
            ASSERT_EQ(TF_OK, TF_GetCode(status), TF_Message(status));
            var value = 0f; // new float[1];
            var t = TFE_TensorHandleResolve(value_handle[0], status);
            ASSERT_EQ(TF_OK, TF_GetCode(status), TF_Message(status));
            ASSERT_EQ(sizeof(float), (int)TF_TensorByteSize(t));
            tf.memcpy(&value, TF_TensorData(t).ToPointer(), sizeof(float));
            c_api.TF_DeleteTensor(t);
            EXPECT_EQ(12.0f, value);

            TFE_DeleteTensorHandle(var_handle);
            TFE_DeleteTensorHandle(value_handle[0]);
            TFE_DeleteContext(ctx);
            CHECK_EQ(TF_OK, TF_GetCode(status), TF_Message(status));
        }
    }
}
