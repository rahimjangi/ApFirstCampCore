﻿using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts;

/// <summary>
/// Represents business logic for maniplating Person Entity Model
/// </summary>
public interface IPersonService
{
    /// <summary>
    /// Adds a new Person object to list of persons
    /// </summary>
    /// <param name="personAddRequest">PersonAddRequest object</param>
    /// <returns>,PersonResponse object</returns>
    PersonResponse? AddPerson(PersonAddRequest? personAddRequest);

    /// <summary>
    /// Returns all existing person from personlist
    /// </summary>
    /// <returns>IEnumerable<PersonResponse></returns>
    IEnumerable<PersonResponse> GetAllPersons();

    /// <summary>
    /// Returns PersonResponse object by PersonId if there is any!
    /// </summary>
    /// <param name="PersonId">PersonId-Guid</param>
    /// <returns>PersonResponse model</returns>
    PersonResponse? GetPersonByPersonId(Guid? PersonId);
}