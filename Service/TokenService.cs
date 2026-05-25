using Microsoft.IdentityModel.Tokens;
using SmartGym.API.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SmartGym.API.Service
{
    public class TokenService
    {

        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(IUser user)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),

            new Claim(ClaimTypes.Email, user.Email),

            new Claim(ClaimTypes.Role,user.Roles.Name)
        };

            // Cria uma chave de segurança usando a Key configurada no Jwt:Key
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)
            );

            // Cria as credenciais de assinatura do token
            // HmacSha256 é o algoritmo utilizado para assinar o JWT
            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );

            // Criação do token JWT
            var token = new JwtSecurityToken(

                // Quem gerou o token
                issuer: _configuration["Jwt:Issuer"],

                // Para quem o token foi criado
                audience: _configuration["Jwt:Audience"],

                // Informações armazenadas dentro do token
                claims: claims,

                // Tempo de expiração do token
                expires: DateTime.UtcNow.AddHours(2),

                // Assinatura digital do token
                signingCredentials: credentials
            );

            // Converte o objeto JWT em string
            // Essa string será enviada para o cliente
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
}}
