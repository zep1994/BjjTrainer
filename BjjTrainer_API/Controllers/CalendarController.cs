﻿using BjjTrainer_API.Models.Calendar;
using BjjTrainer_API.Models.DTO.Calendar;
using BjjTrainer_API.Services_API.Calendar;
using Microsoft.AspNetCore.Mvc;

namespace BjjTrainer_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly CalendarService _calendarService;

        public CalendarController(CalendarService calendarService)
        {
            _calendarService = calendarService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent([FromBody] CalendarEventCreateDTO newEventDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var createdEvent = await _calendarService.CreateEventAsync(newEventDto);
                return CreatedAtAction(nameof(GetEventById), new { id = createdEvent.Id }, createdEvent);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEventById(int id)
        {
            var eventDto = await _calendarService.GetEventByIdAsync(id);
            if (eventDto == null)
                return NotFound();

            return Ok(eventDto);
        }

        [HttpGet("user/{userId}/{year}/{month}")]
        public async Task<IActionResult> GetUserEventsAsync(string userId, int year, int month)
        {
            // Log the received parameters for debugging
            Console.WriteLine($"Received: userId = {userId}, year = {year}, month = {month}");

            try
            {
                var events = await _calendarService.GetUserEventsAsync(userId, year, month);
                return Ok(events);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        // Update an existing event
        [HttpPut("{eventId}")]
        public async Task<IActionResult> UpdateEvent(int eventId, [FromBody] CalendarEvent updatedEvent)
        {
            if (updatedEvent == null || updatedEvent.Id != eventId)
            {
                return BadRequest("Event data mismatch.");
            }

            try
            {
                var eventToUpdate = await _calendarService.UpdateEvent(updatedEvent);
                if (eventToUpdate == null)
                {
                    return NotFound($"Event with ID {eventId} not found.");
                }
                return Ok(eventToUpdate);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Delete an event
        [HttpDelete("{eventId}")]
        public async Task<IActionResult> DeleteEvent(int eventId)
        {
            try
            {
                var result = await _calendarService.DeleteEvent(eventId);
                if (!result)
                {
                    return NotFound($"Event with ID {eventId} not found.");
                }
                return NoContent();
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}