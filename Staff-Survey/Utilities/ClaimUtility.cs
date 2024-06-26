﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Staff_Survey.Utilities
{
    public static class ClaimUtility
    {
        public static string GetUserId(ClaimsPrincipal User)
        {
            if (User != null)
            {
                var claimsIdentity = User.Identity as ClaimsIdentity;

                if (claimsIdentity.FindFirst(ClaimTypes.NameIdentifier) != null)
                {
                    string userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                    return userId;
                }
                else
                {
                    return null;
                }
            }
            return null;
        }

        public static string GetUserFullName(ClaimsPrincipal User)
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;

            if (claimsIdentity.FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                return claimsIdentity.FindFirst(c => c.Type.Contains("FullName")).Value;

            }
            else
            {
                return null;
            }
        }


        public static string GetUserEmail(ClaimsPrincipal User)
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;

            if (claimsIdentity.FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                return claimsIdentity.FindFirst(ClaimTypes.Email).Value;
            }
            else
            {
                return null;
            }
        }

        public static List<string> GetRoles(ClaimsPrincipal User)
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            List<string> Roles = new List<string>();
            foreach (var item in claimsIdentity.Claims.Where(c=>c.Type.EndsWith("role")))
            {
                Roles.Add(item.Value);
            }
            return Roles;
        }
    }
}
