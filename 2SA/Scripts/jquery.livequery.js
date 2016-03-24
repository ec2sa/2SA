/*! Copyright (c) 2008 Brandon Aaron (http://brandonaaron.net)
 * Dual licensed under the MIT (http://www.opensource.org/licenses/mit-license.php) 
 * and GPL (http://www.opensource.org/licenses/gpl-license.php) licenses.
 *
 * Version: 1.0.3
 * Requires jQuery 1.1.3+
 * Docs: http://docs.jquery.com/Plugins/livequery
 */

(function($) {
	
$.extend($.fn, {
	livequery: function(type, fn, fn2) {
		var self = this, q;
		
		// Handle different call patterns
		if ($.isFunction(type))
			fn2 = fn, fn = type, type = undefined;
			
		// See if Live Query already exists
		$.each( $.livequery.queries, function(i, query) {
			if ( self.selector == query.selector && self.context == query.context &&
				type == query.type && (!fn || fn.$lqguid == query.fn.$lqguid) && (!fn2 || fn2.$lqguid == query.fn2.$lqguid) )
					// Found the query, exit the each loop
					return (q = query) && false;
		});
		
		// Create new Live Query if it wasn't found
		q = q || new $.livequery(this.selector, this.context, type, fn, fn2);
		
		// Make sure it is running
		q.stopped = false;
		
		// Run it immediately for the first time
		q.run();
		
		// Contnue the chain
		return this;
	},
	
	expire: function(type, fn, fn2) {
		var self = this;
		
		// Handle different call patterns
		if ($.isFunction(type))
			fn2 = fn, fn = type, type = undefined;
			
		// Find the Live Query based on arguments and stop it
		$.each( $.livequery.queries, function(i, query) {
			if ( self.selector == query.selector && self.context == query.context && 
				(!type || type == query.type) && (!fn || fn.$lqguid == query.fn.$lqguid) && (!fn2 || fn2.$lqguid == query.fn2.$lqguid) && !this.stopped )
					$.livequery.stop(query.id);
		});
		
		// Continue the chain
		return this;
	}
});

$.livequery = function(selector, context, type, fn, fn2) {
	this.selector = selector;
	this.context  = context || document;
	this.type     = type;
	this.fn       = fn;
	this.fn2      = fn2;
	this.elements = [];
	this.stopped  = false;
	
	// The id is the index of the Live Query in $.livequery.queries
	this.id = $.livequery.queries.push(this)-1;
	
	// Mark the functions for matching later on
	fn.$lqguid = fn.$lqguid || $.livequery.guid++;
	if (fn2) fn2.$lqguid = fn2.$lqguid || $.livequery.guid++;
	
	// Return the Live Query
	return this;
};

$.livequery.prototype = {
	stop: function() {
		var query = this;
		
		if ( this.type )
			// Unbind all bound events
			this.elements.unbind(this.type, this.fn);
		else if (this.fn2)
			// Call the second function for all matched elements
			this.elements.each(function(i, el) {
				query.fn2.apply(el);
			});
			
		// Clear out matched elements
		this.elements = [];
		
		// Stop the Live Query from running until restarted
		this.stopped = true;
	},
	
	run: function() {
		// Short-circuit if stopped
		if ( this.stopped ) return;
		var query = this;
		
		var oEls = this.elements,
			els  = $(this.selector, this.context),
			nEls = els.not(oEls);
		
		// Set elements to the latest set of matched elements
		this.elements = els;
		
		if (this.type) {
			// Bind events to newly matched elements
			nEls.bind(this.type, this.fn);
			
			// Unbind events to elements no longer matched
			if (oEls.length > 0)
				$.each(oEls, function(i, el) {
					if ( $.inArray(el, els) < 0 )
						$.event.remove(el, query.type, query.fn);
				});
		}
		else {
			// Call the first function for newly matched elements
			nEls.each(function() {
				query.fn.apply(this);
			});
			
			// Call the second function for elements no longer matched
			if ( this.fn2 && oEls.length > 0 )
				$.each(oEls, function(i, el) {
					if ( $.inArray(el, els) < 0 )
						query.fn2.apply(el);
				});
		}
	}
};

$.extend($.livequery, {
	guid: 0,
	queries: [],
	queue: [],
	running: false,
	timeout: null,
	
	checkQueue: function() {
		if ( $.livequery.running && $.livequery.queue.length ) {
			var length = $.livequery.queue.length;
			// Run each Live Query currently in the queue
			while ( length-- )
				$.livequery.queries[ $.livequery.queue.shift() ].run();
		}
	},
	
	pause: function() {
		// Don't run anymore Live Queries until restarted
		$.livequery.running = false;
	},
	
	play: function() {
		// Restart Live Queries
		$.livequery.running = true;
		// Request a run of the Live Queries
		$.livequery.run();
	},
	
	registerPlugin: function() {
		$.each( arguments, function(i,n) {
			// Short-circuit if the method doesn't exist
			if (!$.fn[n]) return;
			
			// Save a reference to the original method
			var old = $.fn[n];
			
			// Create a new method
			$.fn[n] = function() {
				// Call the original method
				var r = old.apply(this, arguments);
				
				// Request a run of the Live Queries
				$.livequery.run();
				
				// Return the original methods result
				return r;
			}
		});
	},
	
	run: function(id) {
		if (id != undefined) {
			// Put the particular Live Query in the queue if it doesn't already exist
			if ( $.inArray(id, $.livequery.queue) < 0 )
				$.livequery.queue.push( id );
		}
		else
			// Put each Live Query in the queue if it doesn't already exist
			$.each( $.livequery.queries, function(id) {
				if ( $.inArray(id, $.livequery.queue) < 0 )
					$.livequery.queue.push( id );
			});
		
		// Clear timeout if it already exists
		if ($.livequery.timeout) clearTimeout($.livequery.timeout);
		// Create a timeout to check the queue and actually run the Live Queries
		$.livequery.timeout = setTimeout($.livequery.checkQueue, 20);
	},
	
	stop: function(id) {
		if (id != undefined)
			// Stop are particular Live Query
			$.livequery.queries[ id ].stop();
		else
			// Stop all Live Queries
			$.each( $.livequery.queries, function(id) {
				$.livequery.queries[ id ].stop();
			});
	}
});

// Register core DOM manipulation methods
$.livequery.registerPlugin('append', 'prepend', 'after', 'before', 'wrap', 'attr', 'removeAttr', 'addClass', 'removeClass', 'toggleClass', 'empty', 'remove');

// Run Live Queries when the Document is ready
$(function() { $.livequery.play(); });


// Save a reference to the original init method
var init = $.prototype.init;

// Create a new init method that exposes two new properties: selector and context
$.prototype.init = function(a,c) {
	// Call the original init and save the result
	var r = init.apply(this, arguments);
	
	// Copy over properties if they exist already
	if (a && a.selector)
		r.context = a.context, r.selector = a.selector;
		
	// Set properties
	if ( typeof a == 'string' )
		r.context = c || document, r.selector = a;
	
	// Return the result
	return r;
};

// Give the init function the jQuery prototype for later instantiation (needed after Rev 4091)
$.prototype.init.prototype = $.prototype;

})(jQuery);

/**
* @author Alexander Farkas
* @ version 1.05
*/
(function($) {

    function getFnIndex(args) {
        var ret = 2;
        $.each(args, function(i, data) {

            if ($.isFunction(data)) {
                ret = i;
                return false;
            }
        });
        return ret;
    }


    (function() {

        var contains = document.compareDocumentPosition ? function(a, b) {
            return a.compareDocumentPosition(b) & 16;
        } : function(a, b) {
            return a !== b && (a.contains ? a.contains(b) : true);
        },
	oldLive = $.fn.live,
	oldDie = $.fn.die;

        function createEnterLeaveFn(fn, type) {
            return jQuery.event.proxy(fn, function(e) {
                if (this !== e.relatedTarget && e.relatedTarget && !contains(this, e.relatedTarget)) {
                    e.type = type;
                    fn.apply(this, arguments);
                }
            });
        }

        var enterLeaveTypes = {
            mouseenter: 'mouseover',
            mouseleave: 'mouseout'
        };

        $.fn.live = function(types) {
            var that = this,
			args = arguments,
			fnIndex = getFnIndex(args),
			fn = args[fnIndex];

            $.each(types.split(' '), function(i, type) {
                var proxy = fn;

                if (enterLeaveTypes[type]) {
                    proxy = createEnterLeaveFn(proxy, type);
                    type = enterLeaveTypes[type];
                }
                args[0] = type;
                args[fnIndex] = proxy;
                oldLive.apply(that, args);
            });
            return this;
        };

        $.fn.die = function(type, fn) {
            if (/mouseenter|mouseleave/.test(type)) {
                if (type == 'mouseenter') {
                    type = type.replace(/mouseenter/g, 'mouseover');
                }
                if (type == 'mouseleave') {
                    type = type.replace(/mouseleave/g, 'mouseout');
                }
            }
            oldDie.call(this, type, fn);
            return this;
        };


        function createBubbleFn(fn, selector, context) {
            return jQuery.event.proxy(fn, function(e) {
                var parent = this.parentNode,
				stop = (enterLeaveTypes[e.type]) ? e.relatedTarget : undefined;
                fn.apply(this, arguments);
                while (parent && parent !== context && parent !== e.relatedTarget) {
                    if ($.multiFilter(selector, [parent])[0]) {
                        fn.apply(parent, arguments);
                    }
                    parent = parent.parentNode;
                }
            });
        }

        $.fn.bubbleLive = function() {
            var args = arguments,
			fnIndex = getFnIndex(args);

            args[fnIndex] = createBubbleFn(args[fnIndex], this.selector, this.context);
            $.fn.live.apply(this, args);
        };

        $.fn.liveHover = function(enter, out) {
            return this.live('mouseenter', enter)
					.live('mouseleave', out);
        };
    })();



    (function() {

        $.support.bubblingChange = !($.browser.msie || $.browser.safari);

        if (!$.support.bubblingChange) {

            var oldLive = $.fn.live,
		oldDie = $.fn.die;

            function detectChange(fn) {
                return $.event.proxy(fn, function(e) {
                    var jElm = $(e.target);
                    if ((e.type !== 'keydown' || e.keyCode === 13) && jElm.is('input, textarea, select')) {

                        var oldData = jElm.data('changeVal'),
					isRadioCheckbox = jElm.is(':checkbox, :radio'),
					nowData;
                        if (isRadioCheckbox && jElm.is(':enabled') && e.type === 'click') {
                            nowData = jElm.is(':checked');
                            if ((e.target.type !== 'radio' || nowData === true) && e.type !== 'change' && oldData !== nowData) {
                                e.type = 'change';
                                jElm.trigger(e);
                            }
                        } else if (!isRadioCheckbox) {
                            nowData = jElm.val();
                            if (oldData !== undefined && oldData !== nowData) {
                                e.type = 'change';
                                jElm.trigger(e);
                            }
                        }
                        if (nowData !== undefined) {
                            jElm.data('changeVal', nowData);
                        }
                    }
                });
            }

            function createChangeProxy(fn) {
                return $.event.proxy(fn, function(e) {
                    if (e.type === 'change') {
                        var jElm = $(e.target),
					nowData = (jElm.is(':checkbox, :radio')) ? jElm.is(':checked') : jElm.val();
                        if (nowData === jElm.data('changeVal')) {
                            return false;
                        }
                        jElm.data('changeVal', nowData);
                    }
                    fn.apply(this, arguments);
                });
            }

            $.fn.live = function(type, fn) {
                var that = this,
			args = arguments,
			fnIndex = getFnIndex(args),
			proxy = args[fnIndex];

                if (type.indexOf('change') != -1) {
                    $(this.context)
				.bind('click focusin focusout keydown', detectChange(proxy));
                    proxy = createChangeProxy(proxy);
                }
                args[fnIndex] = proxy;
                oldLive.apply(that, args);
                return this;
            };
            $.fn.die = function(type, fn) {
                if (type.indexOf('change') != -1) {
                    $(this.context)
				.unbind('click focusin focusout keydown', fn);
                }
                oldDie.apply(this, arguments);
                return this;
            };

        }
    })();

    /**
    * Copyright (c) 2007 Jörn Zaefferer
    */


    (function() {
        $.support.focusInOut = !!($.browser.msie);
        if (!$.support.focusInOut) {
            $.each({
                focus: 'focusin',
                blur: 'focusout'
            }, function(original, fix) {
                $.event.special[fix] = {
                    setup: function() {
                        if (!this.addEventListener) {
                            return false;
                        }
                        this.addEventListener(original, $.event.special[fix].handler, true);
                    },
                    teardown: function() {
                        if (!this.removeEventListener) {
                            return false;
                        }
                        this.removeEventListener(original, $.event.special[fix].handler, true);
                    },
                    handler: function(e) {
                        arguments[0] = $.event.fix(e);
                        arguments[0].type = fix;
                        return $.event.handle.apply(this, arguments);
                    }
                };
            });
        }
        //IE has some troubble with focusout with select and keyboard navigation
        var activeFocus = null, block;

        $(document)
		.bind('focusin', function(e) {
		    var target = e.realTarget || e.target;
		    if (activeFocus && activeFocus !== target) {
		        e.type = 'focusout';
		        $(activeFocus).trigger(e);
		        e.type = 'focusin';
		        e.target = target;
		    }
		    activeFocus = target;
		})
		.bind('focusout', function(e) {
		    activeFocus = null;
		});

    })();
})(jQuery);