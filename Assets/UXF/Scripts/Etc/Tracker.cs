using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

namespace UXF
{
    /// <summary>
    /// Create a new class that inherits from this component to create custom tracking behaviour on a frame-by-frame basis.
    /// </summary>
    public abstract class Tracker : MonoBehaviour
    {
        /// <summary>
        /// Name of the object used in saving
        /// </summary>
        public string objectName;

        /// <summary>
        /// Description of the type of measurement this tracker will perform.
        /// </summary>
        [Tooltip("Description of the type of measurement this tracker will perform.")]
        public string measurementDescriptor;

        /// <summary>
        /// Custom column headers for tracked objects. Time is added automatically
        /// </summary>
        [Tooltip("Custom column headers for each measurement.")]
        public string[] customHeader = new string[] { };
   
        /// <summary>
        /// A name used when saving the data from this tracker.
        /// </summary>
        public string dataName
        {
            get
            {
                Debug.AssertFormat(measurementDescriptor.Length > 0, "No measurement descriptor has been specified for this Tracker!");
                return string.Join("_", new string[]{ objectName, measurementDescriptor });
            }
        }

        private bool recording;

        public UXFDataTable data { get; private set; } = new UXFDataTable();
        
        /// <summary>
        /// The header that will go at the top of the output file associated with this tracker
        /// </summary>
        /// <returns></returns>
        public string[] header
        { 
            get
            {
                var newHeader = new string[customHeader.Length + 1];
                newHeader[0] = "time";
                customHeader.CopyTo(newHeader, 1);
                return newHeader;
            }
        } 

        // called when component is added
        void Reset()
        {
            objectName = gameObject.name.Replace(" ", "_").ToLower();
            SetupDescriptorAndHeader();
        }

        // called by unity just before rendering the frame
        void LateUpdate()
        {
            if (recording) RecordRow();
        }

        /// <summary>
        /// Records a new row of data at current time.
        /// </summary>
        public void RecordRow()
        {
            UXFDataRow newRow = GetCurrentValues();
            newRow.Add(("time", Time.time));
            data.AddCompleteRow(newRow);
        }


        /// <summary>
        /// Begins recording.
        /// </summary>
        public void StartRecording()
        {
            data = new UXFDataTable(header);
            recording = true;
        }

        /// <summary>
        /// Pauses recording.
        /// </summary>
        public void PauseRecording()
        {
            recording = false;
        }

        /// <summary>
        /// Stops recording.
        /// </summary>
        public void StopRecording()
        {
            recording = false;
        }

        /// <summary>
        /// Acquire values for this frame and store them in an UXFDataRow. Must return values for ALL columns.
        /// </summary>
        /// <returns></returns>
        protected abstract UXFDataRow GetCurrentValues();

        /// <summary>
        /// Override this method and define your own descriptor and header.
        /// </summary>
        protected abstract void SetupDescriptorAndHeader();

    }
}