using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SE161774.ProductManagement.Repo.Helpers;
using SE161774.ProductManagement.Repo.Interface;
using SE161774.ProductManagement.Repo.Models;
using SE161774.ProductManagement.Repo.Repository;
using SE161774.ProductManagement.Repo.ResponeModel;
using SE161774.ProductManagement.Repo.ResponeModels;
using SE161774.ProductManagement.Repo.ViewModel.CategoryViewModel;
using SE161774.ProductManagement.Repo.ViewModels.CategoryViewModels;
using SE161774.ProductManagement.Repo.ViewModels.ProductViewModel;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SE161774.ProductManagement.API.Controllers
{
    [Route("api/products")]
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
                    return BadRequest(new FailedResponseModel()
                    {
                        Status = StatusCodes.Status404NotFound,
                        Message = "Product not exist with id " + id
                    });
                }
                var category = await _unitOfWork.CategorysRepository.GetByIdAsync((int)result.CategoryId);
                var productViewModel = _mapper.Map<ProductViewModel>(result);
                productViewModel.CategoryName = result.Category.CategoryName.ToString();
                return Ok(new ResponeModel
                {
                    Status = StatusCodes.Status200OK,
                    Message = "Get product Succeed",
                    Result = productViewModel
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
        public async Task<IActionResult> GetListProduct([FromQuery] ProductSearchModel search)
        {
            try
            {
                Expression<Func<Product, bool>> filterI = null;
                if (!string.IsNullOrEmpty(search.product_name))
                {
                    filterI = c => c.ProductName.Contains(search.product_name);
                }

                
                if (search.unit_price_min.HasValue || search.unit_price_max.HasValue)
                {
                    if (filterI == null)
                    {
                        filterI = p => true;
                    }
                    if (search.unit_price_min.HasValue)
                    {
                        filterI = filterI.And(p => p.UnitPrice >= search.unit_price_min);
                    }
                    if (search.unit_price_max.HasValue)
                    {
                        filterI = filterI.And(p => p.UnitPrice <= search.unit_price_max);
                    }
                }

                Func<IQueryable<Product>, IOrderedQueryable<Product>> orderByFunc = null;

                if (search.sort_by_price == true && search.descending == true)
                {

                    orderByFunc = p => p.OrderByDescending(p => p.UnitPrice);
                }
                else if (search.sort_by_price == true && search.descending == false)
                {
                    orderByFunc = p => p.OrderBy(p => p.UnitPrice);
                }
                else if (search.sort_by_price == false && search.descending == true)
                {
                    orderByFunc = p => p.OrderByDescending(p => p.ProductName);
                }
                else
                {
                    orderByFunc = p => p.OrderBy(p => p.ProductName);
                }

                string includeProperties = "Category";
                var result = await _unitOfWork.ProductsRepository.ToPaginationAsync(filterI, orderByFunc, includeProperties, search.page_index, search.page_size);

                if (result == null || !result.Items.Any())
                {
                    return BadRequest(new FailedResponseModel()
                    {
                        Status = StatusCodes.Status404NotFound,
                        Message = "Product list has no item"
                    });
                }
                var productViewModel = _mapper.Map<Pagination<ProductViewModel>>(result);

                return Ok(new ResponeModel
                {
                    Status = StatusCodes.Status200OK,
                    Message = "Get category list succeed",
                    Result = productViewModel
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

        [Authorize(Roles = "2")]
        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] ProductModel productModel)
        {
            try
            {
                var exist = await _unitOfWork.ProductsRepository.GetByIdAsync(productModel.ProductId);
                if (productModel.UnitPrice <= 0 || productModel.UnitsInStock <= 0)
                {
                    throw new ArgumentException("UnitPrice and Units In Stock must greater than 0");
                }
                if (exist != null)
                {
                    return BadRequest(new FailedResponseModel()
                    {
                        Status = StatusCodes.Status409Conflict,
                        Message = "Product has exist with id " + productModel.ProductId
                    });
                } 
                var existCT = await _unitOfWork.CategorysRepository.GetByIdAsync(productModel.CategoryId);

                if (existCT == null)
                {
                    return BadRequest(new FailedResponseModel()
                    {
                        Status = StatusCodes.Status404NotFound,
                        Message = "Category not exist with id " + productModel.CategoryId
                    });
                }
                var product = _mapper.Map<Product>(productModel);
                await _unitOfWork.ProductsRepository.AddAsync(product);
                _unitOfWork.Save();
                return Ok(new ResponeModel
                {
                    Status = StatusCodes.Status201Created,
                    Message = "Add product Succeed",
                    Result = productModel
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new FailedResponseModel()
                {
                    Status = BadRequest().StatusCode,
                    Message = ex.Message,
                });
            }
        }

        [Authorize(Roles = "2")]
        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateProductById(int Id, [FromBody] ProductModel productModel)
        {
            try
            {
                var product = await _unitOfWork.ProductsRepository.GetByIdAsync(Id);
                if (productModel.UnitPrice <= 0 || productModel.UnitsInStock <= 0)
                {
                    throw new ArgumentException("UnitPrice and Units In Stock must greater than 0");
                }
                if (product == null)
                {
                    return BadRequest(new FailedResponseModel()
                    {
                        Status = StatusCodes.Status404NotFound,
                        Message = "Product not exist with id " + Id
                    });
                }
                var existCT = await _unitOfWork.CategorysRepository.GetByIdAsync(productModel.CategoryId);

                if(product.UnitPrice <= 0 || product.UnitsInStock <= 0)
                {
                    throw new ArgumentException("UnitPrice and Units In Stock must greater than 0");
                }

                if (existCT == null)
                {
                    return BadRequest(new FailedResponseModel()
                    {
                        Status = StatusCodes.Status404NotFound,
                        Message = "Category not exist with id " + productModel.CategoryId
                    });
                }
                _mapper.Map(productModel, product);
                _unitOfWork.ProductsRepository.Update(product);
                _unitOfWork.Save();
                var result = _mapper.Map<ProductViewModel>(product);
                return Ok(new ResponeModel
                {
                    Status = StatusCodes.Status200OK,
                    Message = "Update product Succeed",
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
        public async Task<IActionResult> DeleteProductById(int id)
        {
            try
            {
                var result = await _unitOfWork.ProductsRepository.GetByIdAsync(id);
                if (result == null)
                {
                    return BadRequest(new FailedResponseModel()
                    {
                        Status = StatusCodes.Status404NotFound,
                        Message = "Product not exist with id " + id
                    });
                }
                _unitOfWork.ProductsRepository.Remove(result);
                _unitOfWork.Save();
                return Ok(new ResponeModel
                {
                    Status = StatusCodes.Status200OK,
                    Message = "Delete product Succeed",
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


