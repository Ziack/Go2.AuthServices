﻿using Go2.AuthServices.Internal;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Metadata;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Go2.AuthServices.Metadata
{
    /// <summary>
    /// Helper for loading SAML2 metadata
    /// </summary>
    public static class MetadataLoader
    {
        /// <summary>
        /// Load and parse metadata.
        /// </summary>
        /// <param name="metadataLocation">Path to metadata. A Url, absolute
        /// path or an app relative path (e.g. ~/App_Data/metadata.xml)</param>
        /// <returns>EntityDescriptor containing metadata</returns>
        public static ExtendedEntityDescriptor LoadIdp(string metadataLocation)
        {
            return LoadIdp(metadataLocation, false);
        }
        
        internal const string LoadIdpFoundEntitiesDescriptor = "Tried to load metadata for an IdentityProvider, which should be an <EntityDescriptor>, but found an <EntitiesDescriptor>. To load that metadata you should use the Federation configuration and not an IdentityProvider. You can also set the SPOptions.Compatibility.UnpackEntitiesDescriptorInIdentityProviderMetadata option to true.";
        internal const string LoadIdpUnpackingFoundMultipleEntityDescriptors = "Unpacked an EntitiesDescriptor when loading idp metadata, but found multiple EntityDescriptors.Unpacking is only supported if the metadata contains a single EntityDescriptor. Maybe you should use a Federation instead of configuring a single IdentityProvider";

        /// <summary>
        /// Load and parse metadata.
        /// </summary>
        /// <param name="metadataLocation">Path to metadata. A Url, absolute
        /// path or an app relative path (e.g. ~/App_Data/metadata.xml)</param>
        /// <param name="unpackEntitiesDescriptor">If the metadata contains
        /// an EntitiesDescriptor, try to unpack it and return a single
        /// EntityDescriptor inside if there is one.</param>
        /// <returns>EntityDescriptor containing metadata</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "EntityDescriptors")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "SPOptions")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "UnpackEntitiesDescriptorInIdentityProviderMetadata")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "EntitiesDescriptor")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "EntityDescriptor")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "IdentityProvider")]
        public static ExtendedEntityDescriptor LoadIdp(string metadataLocation, bool unpackEntitiesDescriptor)
        {
            if (metadataLocation == null)
            {
                throw new ArgumentNullException(nameof(metadataLocation));
            }

            var result = Load(metadataLocation);

            var entitiesDescriptor = result as ExtendedEntitiesDescriptor;
            if(entitiesDescriptor != null)
            {
                if(unpackEntitiesDescriptor)
                {
                    if(entitiesDescriptor.ChildEntities.Count > 1)
                    {
                        throw new InvalidOperationException(LoadIdpUnpackingFoundMultipleEntityDescriptors);
                    }

                    return (ExtendedEntityDescriptor)entitiesDescriptor.ChildEntities.Single();
                }

                throw new InvalidOperationException(LoadIdpFoundEntitiesDescriptor);
            }

            return (ExtendedEntityDescriptor)result;
        }

        private static MetadataBase Load(string metadataLocation)
        {
            if(PathHelper.IsWebRootRelative(metadataLocation))
            {
                metadataLocation = PathHelper.MapPath(metadataLocation);
            }

            using (var client = new WebClient())
            using (var stream = client.OpenRead(metadataLocation))
            {
                return Load(stream);
            }
        }

        internal static MetadataBase Load(Stream metadataStream)
        {
            var serializer = ExtendedMetadataSerializer.ReaderInstance;
            using (var reader = XmlDictionaryReader.CreateTextReader(metadataStream, XmlDictionaryReaderQuotas.Max))
            {
                // Filter out the signature from the metadata, as the built in MetadataSerializer
                // doesn't handle the XmlDsigNamespaceUrl http://www.w3.org/2000/09/xmldsig# which
                // is allowed (and for SAMLv1 even recommended).
                using (var filter = new FilteringXmlDictionaryReader(SignedXml.XmlDsigNamespaceUrl, "Signature", reader))
                {
                    return serializer.ReadMetadata(filter);
                }
            }
        }

        internal const string LoadFederationFoundEntityDescriptor = "Tried to load metadata for a Federation, which should be an <EntitiesDescriptor> containing one or more <EntityDescriptor> elements, but found an <EntityDescriptor>. To load that metadata you should use the IdentityProvider configuration and not a Federation.";

        /// <summary>
        /// Load and parse metadata for a federation.
        /// </summary>
        /// <param name="metadataLocation">Url to metadata</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "EntitiesDescriptor")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "EntityDescriptor")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "IdentityProvider")]
        public static ExtendedEntitiesDescriptor LoadFederation(string metadataLocation)
        {
            if (metadataLocation == null)
            {
                throw new ArgumentNullException(nameof(metadataLocation));
            }

            var result = Load(metadataLocation);

            if(result is ExtendedEntityDescriptor)
            {
                throw new InvalidOperationException(LoadFederationFoundEntityDescriptor);
            }

            return (ExtendedEntitiesDescriptor)result;
        }
    }
}
