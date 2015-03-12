using System;

namespace PocUnifiedLogWorkerCommon
{
    public class UnifiedLogEvent
    {
        /// <summary>
        /// The record ID. This is assigned by the unified log infrastructure.
        /// </summary>
        public virtual long Id { get; set; }

        /// <summary>
        /// Date/time the event was created.
        /// </summary>
        public virtual DateTime CreatedAt { get; set; }

        /// <summary>
        /// Source of the event.
        /// </summary>
        public virtual string Source { get; set; }

        /// <summary>
        /// IP address associated with the event.
        /// </summary>
        public virtual string IpAddress { get; set; }

        /// <summary>
        /// Name of the event.
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Version of the event.
        /// </summary>
        public virtual string Version { get; set; }

        /// <summary>
        /// Data of the event.
        /// </summary>
        public virtual string Data { get; set; }
    }
}
