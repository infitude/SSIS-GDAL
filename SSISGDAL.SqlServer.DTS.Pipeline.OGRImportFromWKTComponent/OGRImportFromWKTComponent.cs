﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Microsoft.SqlServer.Dts.Pipeline;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using Microsoft.SqlServer.Dts.Runtime.Wrapper;
using Microsoft.SqlServer.Dts.Runtime;
using OSGeo.OGR;
using SSISGDAL.SqlServer.DTS.SSISGDAL;

namespace SSISGDAL.SqlServer.DTS.Pipeline
{
    [DtsPipelineComponent(
        DisplayName = "OGR ImportFromWKT",
        ComponentType = ComponentType.Transform,
        IconResource = "SSISGDAL.SqlServer.DTS.Pipeline.SSISGDAL.ico"
    )]
    public class OGRImportFromWKTComponent : OGRTransformation
    {
        public override void ProvideComponentProperties()
        {
            base.ProvideComponentProperties();

            IDTSOutput100 defaultOutput;
            if (ComponentMetaData.OutputCollection[0].IsErrorOut)
            {
                defaultOutput = ComponentMetaData.OutputCollection[1];
            }
            else
            {
                defaultOutput = ComponentMetaData.OutputCollection[0];
            }

            //Add output column
            IDTSOutputColumn100 areaColumn = defaultOutput.OutputColumnCollection.New();
            areaColumn.Name = "OGRGeometry";
            areaColumn.Description = "OGRGeometry";
            areaColumn.SetDataTypeProperties(Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_IMAGE, 0, 0, 0, 0);
        }

        public override DTSValidationStatus Validate()
        {
            DTSValidationStatus validation = base.Validate();

            if (validation == DTSValidationStatus.VS_ISVALID)
            {
                bool cancel;

                //Validate input column
                if (ComponentMetaData.InputCollection[0].InputColumnCollection[0].DataType != DataType.DT_NTEXT)
                {
                    ComponentMetaData.FireError(0, ComponentMetaData.Name, "Invalid input column data type", string.Empty, 0, out cancel);
                    return DTSValidationStatus.VS_ISBROKEN;
                }
            }
            return validation;
        }

        public override void transform(ref PipelineBuffer buffer, int defaultOutputId, int inputColumnBufferIndex, int outputColumnBufferIndex)
        {
            //Get WKT from buffer
            String wkt = buffer.GetString(inputColumnBufferIndex);

            Geometry geom = Geometry.CreateFromWkt(wkt);

            byte[] geomBytes = new byte[geom.WkbSize()];
            geom.ExportToWkb(geomBytes);
            buffer.AddBlobData(outputColumnBufferIndex, geomBytes);
            buffer.DirectRow(defaultOutputId);
        }
    }
}
