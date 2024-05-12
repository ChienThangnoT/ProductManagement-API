using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SE161774.ProductManagement.Repo.Interface;
using SE161774.ProductManagement.Repo.Models;
using SE161774.ProductManagement.Repo.ResponeModel;
using SE161774.ProductManagement.Repo.ResponeModels;
using SE161774.ProductManagement.Repo.ViewModel.CategoryViewModel;
using SE161774.ProductManagement.Repo.ViewModels.CategoryViewModels;

namespace SE161774.ProductManagement.API.Controllers
{
    [Route("api/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("get-category/{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            try
            {
                var result = await _unitOfWork.CategorysRepository.GetByIdAsync(id);
                if (result == null)
                {
                    throw new KeyNotFoundException("Category not found");
                }
                var categoryViewModel = _mapper.Map<CategoryViewModel>(result);
                return Ok(new ResponeModel
                {
                    Status = Ok().StatusCode,
                    Message = "Get category Succeed",
                    Result = categoryViewModel
                });
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(new FailedResponseModel()
                {
                    Status = BadRequest().StatusCode,
                    Message = ex.Message
                });
            }
        }
        
        [HttpPost("add-category")]
        public async Task<IActionResult> AddCategory(CategoryViewModel categoryViewModel)
        {
            try
            {
                var exist = await _unitOfWork.CategorysRepository.GetByIdAsync(categoryViewModel.CategoryId);
                if (exist != null)
                {
                    return BadRequest(new FailedResponseModel()
                    {
                        Status = BadRequest().StatusCode,
                        Message = "Category has exist with id "+ categoryViewModel.CategoryId
                    });
                }
                var category = _mapper.Map<Category>(categoryViewModel);
                await _unitOfWork.CategorysRepository.AddAsync(category);
                _unitOfWork.Save();
                return Ok(new ResponeModel
                {
                    Status = Ok().StatusCode,
                    Message = "Add category Succeed",
                    Result = categoryViewModel
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new FailedResponseModel()
                {
                    Status = BadRequest().StatusCode,
                    Message = ex.Message
                });
            }
        }
        
        [HttpPut("update-category/{Id}")]
        public async Task<IActionResult> UpdateCategoryById(int Id, CategoryUpdateModel categoryUpdate)
        {
            try
            {
                var category = await _unitOfWork.CategorysRepository.GetByIdAsync(Id);
                if (category == null)
                {
                    return BadRequest(new FailedResponseModel()
                    {
                        Status = BadRequest().StatusCode,
                        Message = "Category not exist with id "+ Id
                    });
                }
                _mapper.Map(categoryUpdate, category);
                _unitOfWork.CategorysRepository.Update(category);
                _unitOfWork.Save();
                var result = _mapper.Map<CategoryViewModel>(category);
                return Ok(new ResponeModel
                {
                    Status = Ok().StatusCode,
                    Message = "Update category Succeed",
                    Result = result
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new FailedResponseModel()
                {
                    Status = BadRequest().StatusCode,
                    Message = ex.Message
                });
            }
        }

        [HttpDelete("delete-category/{id}")]
        public async Task<IActionResult> DeleteCategoryById(int id)
        {
            try
            {
                var result = await _unitOfWork.CategorysRepository.GetByIdAsync(id);
                if (result == null)
                {
                    throw new KeyNotFoundException("Category not found");
                }
                _unitOfWork.CategorysRepository.Remove(result);
                _unitOfWork.Save();
                return Ok(new ResponeModel
                {
                    Status = Ok().StatusCode,
                    Message = "Delete category Succeed",
                });
            }
            catch (KeyNotFoundException ex)
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
