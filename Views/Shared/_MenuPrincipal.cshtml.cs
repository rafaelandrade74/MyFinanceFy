using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace MyFinanceFy.Views.Shared
{
    public class _MenuPrincipal : PageModel
    {
        private readonly ILogger<_MenuPrincipal> _logger;

        public _MenuPrincipal(ILogger<_MenuPrincipal> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}