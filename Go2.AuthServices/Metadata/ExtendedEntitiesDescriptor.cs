﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Go2.AuthServices.Metadata
{
    /// <summary>
    /// Extended entitiesdescriptor for SAML2 metadata, adding more attributes.
    /// </summary>
    public class ExtendedEntitiesDescriptor : EntitiesDescriptor, ICachedMetadata
    {
        /// <summary>
        /// Permitted cache duration for the metadata.
        /// </summary>
        public TimeSpan? CacheDuration { get; set; }

        /// <summary>
        /// Valid until
        /// </summary>
        public DateTime? ValidUntil { get; set; }
    }
}
