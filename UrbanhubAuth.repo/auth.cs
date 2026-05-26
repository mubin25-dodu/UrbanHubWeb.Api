using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Design;
using UrbanHub.custom_services;
using UrbanHub.Data;
using UrbanHub.DTO;
using UrbanHub.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UrbanHubManagement.repo
{
    public class Auth(UrbanHubDbContext context)
    {
        public Result<User> IsUser(LoginDTO data)
        {
            var result = new Result<User>();
            try
            {
                var check = context.Users.FirstOrDefault(x => x.Email == data.Email);
                if (check == null)
                {
                    result.Data = null;
                    result.Message = "No User Found";
                    result.Error = true;
                }
                else if (check != null && check.Email!=data.Email)
                {
                    result.Data = null;
                    result.Message = "Wrong email Try again";
                    result.Error = true;
                }
                else if (check != null && check.Password != data.Password)
                {
                    result.Data = null;
                    result.Message = "Wrong password Try again";
                    result.Error = true;
                }
                else if(check.Status.ToLower() =="banned"){
                    result.Data = null;
                    result.Message = "The User Is Banned";
                    result.Error = true;
                }
                else
                {
                    result.Data = check;
                    result.Message = "User found";
                    result.Error = false;
                }
            }
            catch (Exception e)
            {
                result.Data = null;
                result.Message = e.ToString();
                result.Error = true;
                throw;
            }
            return result;
        }

        public Result<List<Registration>> Register(Registration data)
        {
            var result = new Result<List<Registration>>();
            try
            {
                var check = context.Users.FirstOrDefault(x => x.Email == data.Email);
                if (check != null)
                {
                    result.Data = null;
                    result.Message = "User already exists";
                    result.Error = true;
                    return result;
                }
                else
                {
                    var checkreg = context.Registrations.FirstOrDefault(x => x.Email != null && x.Email == data.Email);

                    int id = new Random().Next(1, 100000);
                    if (checkreg == null)
                    {
                        var newdata = new Registration()
                        {
                            Email = data.Email,
                            Name = data.Name,
                            Rid = id
                        };
                        context.Registrations.Add(newdata);
                    }
                    else
                    {
                        checkreg.Rid = id;
                        context.Registrations.Update(checkreg);
                    }

                    var registrationLink = $"https://localhost:7019/Registration?email={data.Email}&id={id}";
                    var  mailbody = $@"
                            <!DOCTYPE html>
                            <html>
                            <head>
                                <title>Registration Confirmation</title>
                            </head>
                            <body style=""font-family: Arial, sans-serif; line-height: 1.6; color: #333;"">
                                <div style=""max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #ddd; border-radius: 5px;"">
                                    <h2 style=""color: #007bff;"">Welcome to UrbanHub!</h2>
                                    <p>Dear {data.Name},</p>
                                    <p>Thank you for registering with us! We're excited to have you as part of our community.</p>
                                    <p>Please click the button below to complete your registration and activate your account:</p>
                                    <p style=""text-align: center;"">
                                        <a href='{registrationLink}' style=""background-color: #007bff; color: #ffffff; padding: 10px 20px; text-decoration: none; border-radius: 5px; display: inline-block;"">Complete Registration</a>
                                    </p>
                                       OR Enter the OTP-{id}
                                    <p>If you're having trouble with the button, you can also copy and paste the following link into your browser:</p>
                                    <p><a href='{registrationLink}'>{registrationLink}</a></p>
                                    <p>If you did not request this registration, please ignore this email.</p>
                                    <hr style=""border: 0; border-top: 1px solid #eee;"">
                                    <p style=""font-size: 0.9em; color: #777;"">Best regards,<br>The UrbanHub Team</p>
                                </div>
                            </body>
                            </html>";

                    new SendMail().SendEmail(data.Email , "UrbanHub - Registration Confirmation", mailbody);

                    context.SaveChanges();
                    //mail sending
                    result.Data = null;
                    result.Message = "An Email hase been send to you please confirm";
                    result.Error = false;
                }
            }
            catch (Exception e)
            {
                result.Message = e.ToString();
                result.Error = true;
                throw;
            }

            return result;
        }

        public Result<Registration> CheckRegistrationEmail(Registration data )
        {
            var result = new Result<Registration>();
            var check = context.Registrations.Where(u => u.Email == data.Email && u.Rid == data.Rid);

            if (!check.Any())
            {
                result.Message = "No User Found";
                result.Error = true;
                return result ;
            }

            result.Error = false;
            result.Message = "User Found";
            return result;


        }

        public Result<UserDTO> Save(UserDTO data)
        {
            var result = new Result<UserDTO>();
            try
            {
                var check = context.Registrations.Where(u => u.Email == data.Email);
                var usercheck = context.Users.FirstOrDefault(e => e.Email == data.Email);
                
                if (usercheck != null)
                {
                    if (usercheck.Email == data.Email)
                    {
                        result.Data = data;
                        result.Error = true;
                        result.Message = "Email already Registered";
                    }
                    else if (usercheck.Phone == data.Phone)
                    {
                        result.Data = data;
                        result.Error = true;
                        result.Message = "Phone Number already Registered";
                    }
                }
                else if (check.Count() != 0 && usercheck == null)
                {
                    data.JoinDate = DateTime.Now;
                    context.Users.Add(new User()
                    {
                        Name = data.Name,
                        Email = data.Email,
                        Password = data.Password,
                        Address = data.Address,
                        Role = "User",
                        Status = "Active",
                        JoinDate = DateTime.Now,
                        Phone = data.Phone
                    });
                    context.Registrations.Remove(check.First());
                    context.SaveChanges();

                    result.Data = null;
                    result.Error = false;
                    result.Message = "Registration Successful";
                }
                return result;
            }
            catch (Exception e)
            {
                result.Data = data;
                result.Error = true;
                result.Message = e.ToString();
                throw;
            }

        }
    }

}
