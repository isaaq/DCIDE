// Decompiled with JetBrains decompiler
// Type: DC.IDE.UI.Diagram.Table.SamplesFactory
// Assembly: DC.IDE.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D536A62-DD1E-45F0-ABC8-30BF28EFB831
// Assembly location: Y:\codes\win\DCIDE\TestContainer\bin\Debug\DC.IDE.UI.dll

using DC.IDE.UI.Util;
using System;
using System.Collections.Generic;
using System.Windows.Threading;
using Telerik.Windows.Controls;

namespace DC.IDE.UI.Diagram.Table
{
    public static class SamplesFactory
    {
        public static IEnumerable<SampleItem> GetSamples(RadDiagram diagram = null)
        {
            return (IEnumerable<SampleItem>)new SampleItem[6]
            {
        new SampleItem()
        {
          Title = "Flow Chart",
          Description = "Example of a work flow diagram.",
          Icon = "../Images/Diagrams/FirstLook/flowchart220.png",
          Run = new Action<RadDiagram>(SamplesFactory.FlowChartSample)
        },
        new SampleItem()
        {
          Title = "Cycle Diagram",
          Description = "Example of a cycle process aka methodology.",
          Icon = "../Images/Diagrams/FirstLook/circle.jpg",
          Run = new Action<RadDiagram>(SamplesFactory.CycleSample)
        },
        new SampleItem()
        {
          Title = "Bezier Diagram",
          Description = "Sample demonstrating a stakeholder diagram.",
          Icon = "../Images/Diagrams/FirstLook/bezier.jpg",
          Run = new Action<RadDiagram>(SamplesFactory.BezierSample)
        },
        new SampleItem()
        {
          Title = "Linear Process Diagram",
          Description = "Linear sequence of dependence.",
          Icon = "../Images/Diagrams/FirstLook/rolls.jpg",
          Run = new Action<RadDiagram>(SamplesFactory.SequenceSample)
        },
        new SampleItem()
        {
          Title = "Floor plan",
          Description = "Sample which demonstrates the possibility to use RadDiagram to create floor plans.",
          Icon = "../Images/Diagrams/FirstLook/floorplan.jpg",
          Run = new Action<RadDiagram>(SamplesFactory.FloorPlanSample)
        },
        new SampleItem()
        {
          Title = "Decision Flowchart",
          Description = "A typical flowchart using RadDiagram.",
          Icon = "../Images/Diagrams/FirstLook/simpleflow.jpg",
          Run = new Action<RadDiagram>(SamplesFactory.SimpleFlowSample)
        }
            };
        }

        public static void StakeholderSample(RadDiagram diagram)
        {
            SamplesFactory.LoadSample(diagram, "Stakeholder");
        }

        public static void SimpleFlowSample(RadDiagram diagram)
        {
            SamplesFactory.LoadSample(diagram, "SimpleFlow");
        }

        public static void FloorPlanSample(RadDiagram diagram)
        {
            SamplesFactory.LoadSample(diagram, "FloorPlan");
        }

        public static void CycleSample(RadDiagram diagram)
        {
            SamplesFactory.LoadSample(diagram, "Cycle3");
        }

        public static void BezierSample(RadDiagram diagram)
        {
            SamplesFactory.LoadSample(diagram, "Supply");
        }

        public static void SequenceSample(RadDiagram diagram)
        {
            SamplesFactory.LoadSample(diagram, "Rolls");
        }

        public static void SimpleDiagramSample(RadDiagram diagram)
        {
            SamplesFactory.LoadSample(diagram, "Flow2");
        }

        public static void FlowChartSample(RadDiagram diagram)
        {
            SamplesFactory.LoadSample(diagram, "FlowChart");
        }

        public static void LoadSample(RadDiagram diagram, string name)
        {
            if (diagram.GraphSource == null)
                diagram.Clear();
            string serializationValue = ResHelper.Load("Diagram.SampleDiagrams.tableshape.xml");
            diagram.Load(serializationValue);
            Action action = (Action)(() => diagram.AutoFit());
            diagram.Dispatcher.BeginInvoke((Delegate)action, DispatcherPriority.ApplicationIdle, (object[])null);
        }
    }
}
