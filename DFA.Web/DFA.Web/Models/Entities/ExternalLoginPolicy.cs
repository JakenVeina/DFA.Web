using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using IdentityModel.Client;

using DFA.Web.Mapping;

namespace DFA.Web.Models.Entities
{
    [MapsTo(typeof(DiscoveryPolicy), nameof(CustomizeMappingFrom))]
    public class ExternalLoginPolicy : EntityBase
    {
        /**********************************************************************/
        #region Properties

        [Required]
        public string Name { get; set; }

        [Required]
        public string Authority { get; set; }

        [Required]
        public string ClientId { get; set; }

        [Required]
        public string ClientSecret { get; set; }

        [Required]
        public bool ValidateIssuerName { get; set; }

        [Required]
        public bool ValidateEndpoints { get; set; }

        public string AdditionalEndpointBaseAddresses { get; set; }

        #endregion Properties

        /**********************************************************************/
        #region Methods

        public static IMappingExpression CustomizeMappingFrom(IMappingExpression configuration)
            => configuration.ForMember(nameof(DiscoveryPolicy.AdditionalEndpointBaseAddresses),
                                       options => options.MapFrom(source => MapAdditionalEndpointBaseAddresses((ExternalLoginPolicy)source)));

        private static ICollection<string> MapAdditionalEndpointBaseAddresses(ExternalLoginPolicy source)
            => source.AdditionalEndpointBaseAddresses?.Split(';') ?? new string[0];

        #endregion Methods
    }
}
