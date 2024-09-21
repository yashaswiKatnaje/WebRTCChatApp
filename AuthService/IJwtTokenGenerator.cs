﻿using Microsoft.AspNetCore.Identity;

public interface IJwtTokenGenerator
{
    string GenerateToken(IdentityUser user);
}
