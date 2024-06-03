using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SE161774.ProductManagement.Repo.Interface;
using SE161774.ProductManagement.Repo.Models;
using SE161774.ProductManagement.Repo.ResponeModel;
using SE161774.ProductManagement.Repo.ResponeModels;
using SE161774.ProductManagement.Repo.ViewModel.CategoryViewModel;
using SE161774.ProductManagement.Repo.ViewModels.CategoryViewModels;
using SE161774.ProductManagement.Repo.ViewModels.ProductViewModel;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.Json;

namespace SE161774.ProductManagement.API.Controllers
{
    [Route("api/categories")]
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

        [HttpGet("{id}")]
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
                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.KebabCaseLower };
                JsonSerializer.Serialize(categoryViewModel, options);
                return Ok(new ResponeModel
                {
                    Status = StatusCodes.Status200OK,
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

        [HttpGet]
        public async Task<IActionResult> GetListCategory(int page_size, int page_index, string? search_by_name)
        {
            try
            {
                Expression<Func<Category, bool>> filterI = null;
                if (search_by_name != null)
                {
                    filterI = c => c.CategoryName.Contains(search_by_name);
                }
                var result = await _unitOfWork.CategorysRepository.GetAsync(filterI, null, "", page_size, page_size);
                if (result == null|| !result.Any())
                {
                    throw new KeyNotFoundException("Category list has no index");
                }
                var categoryViewModel = _mapper.Map<IEnumerable<CategoryViewModel>>(result);

                return Ok(new ResponeModel
                {
                    Status = StatusCodes.Status200OK,
                    Message = "Get category list succeed",
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

        [Authorize(Roles ="2")]
        [HttpPost]
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
                    Status = StatusCodes.Status201Created,
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

        [Authorize(Roles = "2")]
        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateCategoryById(int Id, CategoryUpdateModel categoryUpdate)
        {
            try
            {
                var category = await _unitOfWork.CategorysRepository.GetByIdAsync(Id);
                if (category == null)
                {
                    throw new KeyNotFoundException("Category not found");
                }
                _mapper.Map(categoryUpdate, category);
                _unitOfWork.CategorysRepository.Update(category);
                _unitOfWork.Save();
                var result = _mapper.Map<CategoryViewModel>(category);
                return Ok(new ResponeModel
                {
                    Status = StatusCodes.Status200OK,
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

        [Authorize(Roles = "2")]
        [HttpDelete("{id}")]
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
                    Status = StatusCodes.Status200OK,
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
