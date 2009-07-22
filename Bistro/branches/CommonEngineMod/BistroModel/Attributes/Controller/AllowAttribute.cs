﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Principal;

namespace BistroModel {
	/// <summary>
	/// Marks a controller as allowing a particular permission or role to enter. This attribute negates a higher demand attribute.
	/// </summary>
	public class AllowAttribute : SecurityAttribute {
		/// <summary>
		/// Marks a security controller with permission data
		/// </summary>
		/// <param name="permissionName"></param>
		public AllowAttribute(string role) {
			Role = role;
		}

		/// <summary>
		/// Determines whether the specified user has access.
		/// </summary>
		/// <param name="user">The user.</param>
		/// <returns>
		/// 	<c>true</c> if the specified user has access; otherwise, <c>false</c>.
		/// </returns>
		public override bool HasAccess(IPrincipal user) {
			switch (Role) {
				case ANONYMOUS:
					// allow anonymous users - greedy match, everyone fits
					return user == null || !user.Identity.IsAuthenticated;
				case AUTHENTICATED:
					// allow authed users - so have a profile and be authed
					return user != null && user.Identity.IsAuthenticated;
				default:
					return user.IsInRole(Role);
			}
		}

		/// <summary>
		/// Gets a value indicating whether this permission failing implies that access should be explicitly denied
		/// </summary>
		/// <value><c>true</c> if hard fail; otherwise, <c>false</c>.</value>
		public override bool HardFail { get { return false; } }

		/// <summary>
		/// Gets a value indicating whether this attribute succeeding should override a prior attribute failing
		/// </summary>
		/// <value><c>true</c> if override prior; otherwise, <c>false</c>.</value>
		public override bool OverridePrior { get { return true; } }
	}
}
