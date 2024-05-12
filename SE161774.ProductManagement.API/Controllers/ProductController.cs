using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SE161774.ProductManagement.Repo.Interface;
using SE161774.ProductManagement.Repo.ResponeModel;
using SE161774.ProductManagement.Repo.ResponeModels;
using SE161774.ProductManagement.Repo.ViewModels.ProductViewModel;

namespace SE161774.ProductManagement.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            try
            {
                var result = await _unitOfWork.ProductsRepository.GetByIdAsync(id);
                if (result == null)
                {
                    throw new DirectoryNotFoundException("Product not found");
                }
                var productViewModel = _mapper.Map<ProductViewModel>(result);
                return Ok(new ResponeModel
                {
                    Status = Ok().StatusCode,
                    Message = "Get product Succeed",
                    Result = productViewModel
                });
            }
            catch (DirectoryNotFoundException ex)
            {
                return BadRequest(new FailedResponseModel()
                {
                    Status = BadRequest().StatusCode,
                    Message = ex.Message
                });
            }
        }
    }
}
