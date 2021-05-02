using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SmartChargingApi.Models.Api;

namespace SmartChargingApi.Controllers
{
    public class GroupCapacityValidationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext actionExecutingContext)
        {
            if (actionExecutingContext.ModelState.IsValid == false)
            {
                if (actionExecutingContext.ModelState.ContainsKey("CapacityInAmps"))
                {
                    var groupDto = actionExecutingContext.ActionArguments.Values.First() as GroupDto;

                    var totalCurrent = groupDto
                        .ChargeStations
                        .SelectMany(cs => cs.Connectors)
                        .Aggregate(0f, (total, next) => total + next.MaxCurrentInAmps);

                    var difference = totalCurrent - groupDto.CapacityInAmps;

                    var x = groupDto.ChargeStations
                        .SelectMany(cs => cs.Connectors, (cs, ctor) => new { cs.Name, ctor.MaxCurrentInAmps })
                        .Select(csc => new { ChargeStationName = csc.Name, ConnectorCurrent = csc.MaxCurrentInAmps })
                        .ToList();

                    var res = x.OrderBy(i => Math.Abs(i.ConnectorCurrent - difference))
                        .ThenBy(i => i.ConnectorCurrent > difference).ToList();
                        
                    actionExecutingContext.Result = new BadRequestObjectResult(actionExecutingContext.ModelState);
                }
            }
        }
    }
}
