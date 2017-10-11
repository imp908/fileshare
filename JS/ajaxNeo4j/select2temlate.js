

var data = [{ id: 0, text: 'enhancement' }, { id: 1, text: 'bug' }, { id: 2, text: 'duplicate' }, { id: 3, text: 'invalid' }, { id: 4, text: 'wontfix' }];

$(document).ready(function() {
	console.log("JS3 start load");
	
	//select2 from predefined array
	$(".js-example-data-array").select2({
		data: data
	})

	//select2 from predefined array with selected 2 value
	$(".js-example-data-array-selected").select2({
		data: data
	})
	
	/*
	$(".js-example-data-ajax").select2({
		ajax: {
			
			url:  'https://api.github.com/orgs/select2/repos',
			data: function (params) {
				var query = {
					q: params.term
				}

				// Query parameters will be ?search=[term]&type=public
				return query;
			}			
			,processResults: function (data) {
				console.log(data);
				// Tranforms the top-level key of the response object from 'items' to 'results'
				return {
					results: data.item
				}
			}
			
		}
	})
	*/
	
	$(".js-example-data-ajax").select2({
		
		ajax: {
			url: "https://api.github.com/search/repositories",
			dataType: 'json',
			delay: 250,
			data: function (params) {
			  return {
				q: params.term, // search term
				page: params.page
			  };
			},
			processResults: function (data, params) {
				// parse the results into the format expected by Select2
				// since we are using custom formatting functions we do not need to
				// alter the remote JSON data, except to indicate that infinite
				// scrolling can be used
				params.page = params.page || 1;

			  return {
				results: data.items,
				pagination: {
				  more: (params.page * 30) < data.total_count
				}
			  };
			},
			cache: true
		},
		placeholder: 'Поиск GUID по адресу',
		escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
		minimumInputLength: 3,
		templateResult: formatRepo,
		templateSelection: formatRepoSelection
	  
	});
	
	$('#mySelect2').select2({
		dropdownParent: $('#myModal')
    });
	
function formatRepo (repo) {
  if (repo.loading) {
    return repo.text;
  }

  var markup = "<div class='select2-result-repository clearfix'>" +
    "<div class='select2-result-repository__avatar'><img src='" + repo.owner.avatar_url + "' /></div>" +
    "<div class='select2-result-repository__meta'>" +
      "<div class='select2-result-repository__title'>" + repo.full_name + "</div>";

  if (repo.description) {
    markup += "<div class='select2-result-repository__description'>" + repo.description + "</div>";
  }

  markup += "<div class='select2-result-repository__statistics'>" +
    "<div class='select2-result-repository__forks'><i class='fa fa-flash'></i> " + repo.forks_count + " Forks</div>" +
    "<div class='select2-result-repository__stargazers'><i class='fa fa-star'></i> " + repo.stargazers_count + " Stars</div>" +
    "<div class='select2-result-repository__watchers'><i class='fa fa-eye'></i> " + repo.watchers_count + " Watchers</div>" +
  "</div>" +
  "</div></div>";

  return markup;
}

function formatRepoSelection (repo) {
  return repo.full_name || repo.text;
}

	console.log("JS3 loaded");
	
});
