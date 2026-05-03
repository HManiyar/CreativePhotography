;(function () {
  'use strict';

  /* ── State ──────────────────────────────────────────────────── */
  var state = {
    open: false,
    galleryKey: null,
    index: 0,
    zoom: 1,
    panX: 0,
    panY: 0,
    dragging: false,
    dragX: 0,
    dragY: 0,
    panOriginX: 0,
    panOriginY: 0,
    playing: false,
    playTimer: null,
    pinchDist0: 0,
    pinchZoom0: 1,
  };

  var galleries = {};
  var PLAY_INTERVAL = 3500;
  var el = {}; // cached DOM refs

  /* ── Build viewer HTML ──────────────────────────────────────── */
  function buildUI() {
    var div = document.createElement('div');
    div.id = 'mv-overlay';
    div.setAttribute('role', 'dialog');
    div.setAttribute('aria-modal', 'true');
    div.setAttribute('aria-label', 'Image viewer');
    div.innerHTML =
      '<div id="mv-backdrop"></div>' +
      '<div id="mv-wrap">' +
        '<header id="mv-toolbar">' +
          '<div id="mv-counter" aria-live="polite"></div>' +
          '<div id="mv-actions">' +
            btn('mv-zoom-in',  'Zoom in (+)',          zoomInSVG) +
            btn('mv-zoom-out', 'Zoom out (-)',         zoomOutSVG) +
            btn('mv-zoom-fit', 'Fit image',            fitSVG) +
            btn('mv-play',     'Slideshow (Space)',    playSVG) +
            btn('mv-fullscr',  'Fullscreen (F)',       fullscrSVG) +
            btn('mv-download', 'Download (D)',         downloadSVG) +
            btn('mv-close',    'Close (Esc)',          closeSVG) +
          '</div>' +
        '</header>' +
        '<div id="mv-stage">' +
          '<button id="mv-prev" class="mv-nav" aria-label="Previous image">' + chevronLeft + '</button>' +
          '<div id="mv-frame">' +
            '<div id="mv-spinner"><div class="mv-spin-ring"></div></div>' +
            '<img id="mv-img" alt="" draggable="false">' +
            '<div id="mv-caption"></div>' +
          '</div>' +
          '<button id="mv-next" class="mv-nav" aria-label="Next image">' + chevronRight + '</button>' +
          '<div id="mv-progress"><div id="mv-progress-bar"></div></div>' +
        '</div>' +
        '<nav id="mv-thumbs" aria-label="Thumbnail navigation">' +
          '<div id="mv-thumb-track"></div>' +
        '</nav>' +
      '</div>';

    document.body.appendChild(div);
    cacheRefs();
  }

  function btn(id, title, svgStr) {
    return '<button class="mv-btn" id="' + id + '" title="' + title + '" type="button">' + svgStr + '</button>';
  }

  /* ── SVG icons (Feather / Lucide subset) ────────────────────── */
  var zoomInSVG    = svg('<circle cx="11" cy="11" r="7"/><line x1="16.5" y1="16.5" x2="21" y2="21"/><line x1="11" y1="8" x2="11" y2="14"/><line x1="8" y1="11" x2="14" y2="11"/>');
  var zoomOutSVG   = svg('<circle cx="11" cy="11" r="7"/><line x1="16.5" y1="16.5" x2="21" y2="21"/><line x1="8" y1="11" x2="14" y2="11"/>');
  var fitSVG       = svg('<path d="M8 3H5a2 2 0 0 0-2 2v3m18 0V5a2 2 0 0 0-2-2h-3m0 18h3a2 2 0 0 0 2-2v-3M3 16v3a2 2 0 0 0 2 2h3"/>');
  var playSVG      = '<svg id="mv-play-svg" viewBox="0 0 24 24"><polygon id="mv-play-icon" points="5 3 19 12 5 21 5 3"/></svg>';
  var fullscrSVG   = svg('<polyline points="15 3 21 3 21 9"/><polyline points="9 21 3 21 3 15"/><line x1="21" y1="3" x2="14" y2="10"/><line x1="3" y1="21" x2="10" y2="14"/>');
  var downloadSVG  = svg('<path d="M21 15v4a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2v-4"/><polyline points="7 10 12 15 17 10"/><line x1="12" y1="15" x2="12" y2="3"/>');
  var closeSVG     = svg('<line x1="18" y1="6" x2="6" y2="18"/><line x1="6" y1="6" x2="18" y2="18"/>');
  var chevronLeft  = svg('<polyline points="15 18 9 12 15 6"/>');
  var chevronRight = svg('<polyline points="9 18 15 12 9 6"/>');

  function svg(paths) {
    return '<svg viewBox="0 0 24 24">' + paths + '</svg>';
  }

  /* ── Cache DOM refs ─────────────────────────────────────────── */
  function cacheRefs() {
    function g(id) { return document.getElementById(id); }
    el.overlay     = g('mv-overlay');
    el.backdrop    = g('mv-backdrop');
    el.counter     = g('mv-counter');
    el.frame       = g('mv-frame');
    el.img         = g('mv-img');
    el.spinner     = g('mv-spinner');
    el.caption     = g('mv-caption');
    el.progress    = g('mv-progress');
    el.progressBar = g('mv-progress-bar');
    el.thumbTrack  = g('mv-thumb-track');
    el.thumbsNav   = g('mv-thumbs');
    el.prev        = g('mv-prev');
    el.next        = g('mv-next');
    el.playIcon    = g('mv-play-icon');
  }

  /* ── Open ───────────────────────────────────────────────────── */
  function open(galleryKey, index) {
    state.open = true;
    state.galleryKey = galleryKey;
    resetZoom(true);

    el.overlay.classList.add('mv-is-open');
    document.body.classList.add('mv-no-scroll');

    buildThumbs(galleryKey);
    loadFirst(index);

    requestAnimationFrame(function () {
      el.overlay.classList.add('mv-visible');
    });
  }

  /* ── Close ──────────────────────────────────────────────────── */
  function close() {
    if (!state.open) return;
    stopPlay();
    state.open = false;

    el.overlay.classList.remove('mv-visible');
    setTimeout(function () {
      el.overlay.classList.remove('mv-is-open');
      document.body.classList.remove('mv-no-scroll');
      if (document.fullscreenElement) {
        document.exitFullscreen().catch(function () {});
      }
    }, 350);
  }

  /* ── Load first image (no slide animation) ──────────────────── */
  function loadFirst(index) {
    var items = galleries[state.galleryKey];
    state.index = index;
    resetZoom(true);

    el.spinner.style.display = 'flex';
    el.img.style.opacity = '0';

    var src = items[index].src;
    var tmp = new Image();
    tmp.onload = tmp.onerror = function () {
      el.img.src = src;
      el.img.style.opacity = '1';
      el.spinner.style.display = 'none';
      updateInfo(index);
    };
    tmp.src = src;
  }

  /* ── Navigate with slide animation ─────────────────────────── */
  function navigate(newIndex) {
    var items = galleries[state.galleryKey];
    if (!items || newIndex === state.index) return;

    // Determine slide direction (handles wrap-around naturally)
    var diff = newIndex - state.index;
    if (diff > items.length / 2) diff -= items.length;
    if (diff < -items.length / 2) diff += items.length;
    var dir = diff >= 0 ? 1 : -1;

    var img = el.img;

    // Exit current image
    img.style.transition = 'opacity 0.18s ease, transform 0.22s ease';
    img.style.opacity = '0';
    img.style.transform = 'translateX(' + (dir * -45) + 'px)';

    setTimeout(function () {
      state.index = newIndex;
      resetZoom(true);

      var src = items[newIndex].src;
      var tmp = new Image();

      tmp.onload = tmp.onerror = function () {
        img.src = src;
        el.spinner.style.display = 'none';
        updateInfo(newIndex);

        // Stage entrance position
        img.style.transition = 'none';
        img.style.opacity = '0';
        img.style.transform = 'translateX(' + (dir * 45) + 'px)';

        // Animate entrance
        requestAnimationFrame(function () {
          requestAnimationFrame(function () {
            img.style.transition = 'opacity 0.28s ease, transform 0.34s cubic-bezier(0.25, 0.46, 0.45, 0.94)';
            img.style.opacity = '1';
            img.style.transform = 'translateX(0)';
          });
        });
      };

      el.spinner.style.display = 'flex';
      tmp.src = src;
    }, 210);
  }

  function prev() {
    var items = galleries[state.galleryKey];
    if (!items) return;
    navigate((state.index - 1 + items.length) % items.length);
    if (state.playing) resetPlayTimer();
  }

  function next() {
    var items = galleries[state.galleryKey];
    if (!items) return;
    navigate((state.index + 1) % items.length);
    if (state.playing) resetPlayTimer();
  }

  /* ── Thumbnails ─────────────────────────────────────────────── */
  function buildThumbs(galleryKey) {
    var items = galleries[galleryKey];
    el.thumbTrack.innerHTML = '';

    items.forEach(function (item, i) {
      var t = document.createElement('button');
      t.type = 'button';
      t.className = 'mv-thumb';
      t.setAttribute('aria-label', 'View image ' + (i + 1));

      var img = document.createElement('img');
      img.src = item.thumb || item.src;
      img.alt = '';
      img.loading = 'lazy';

      t.appendChild(img);
      t.addEventListener('click', function () { navigate(i); });
      el.thumbTrack.appendChild(t);
    });

    el.thumbsNav.style.display = items.length > 1 ? '' : 'none';
  }

  function updateInfo(index) {
    var items = galleries[state.galleryKey];
    el.counter.textContent = (index + 1) + ' ⁄ ' + items.length;

    var cap = items[index].caption;
    el.caption.textContent = cap;
    el.caption.style.display = cap ? 'block' : 'none';

    // Update active thumb
    var thumbs = el.thumbTrack.querySelectorAll('.mv-thumb');
    for (var i = 0; i < thumbs.length; i++) {
      thumbs[i].classList.toggle('mv-thumb--active', i === index);
    }
    if (thumbs[index]) {
      thumbs[index].scrollIntoView({ inline: 'center', behavior: 'smooth' });
    }
  }

  /* ── Zoom ───────────────────────────────────────────────────── */
  function setZoom(z, cx, cy) {
    var minZ = 1, maxZ = 5;
    z = Math.max(minZ, Math.min(maxZ, z));

    if (z === 1) {
      state.zoom = 1;
      state.panX = 0;
      state.panY = 0;
    } else if (cx !== undefined && cy !== undefined) {
      var r = el.frame.getBoundingClientRect();
      var ox = cx - r.left - r.width  / 2;
      var oy = cy - r.top  - r.height / 2;
      var scale = z / state.zoom;
      state.panX = state.panX * scale - ox * (scale - 1);
      state.panY = state.panY * scale - oy * (scale - 1);
      state.zoom = z;
    } else {
      state.zoom = z;
    }

    applyTransform();
    el.img.style.cursor = state.zoom > 1 ? 'grab' : 'default';
    el.frame.classList.toggle('mv-frame--zoomed', state.zoom > 1);
  }

  function resetZoom(silent) {
    state.zoom = 1;
    state.panX = 0;
    state.panY = 0;
    if (!silent) {
      applyTransform();
      el.img.style.cursor = 'default';
      el.frame.classList.remove('mv-frame--zoomed');
    }
  }

  function applyTransform() {
    el.img.style.transform =
      'translate(' + state.panX + 'px, ' + state.panY + 'px) scale(' + state.zoom + ')';
  }

  /* ── Autoplay ───────────────────────────────────────────────── */
  function startPlay() {
    state.playing = true;
    el.playIcon.innerHTML = '<rect x="6" y="4" width="4" height="16"/><rect x="14" y="4" width="4" height="16"/>';
    el.progress.style.display = 'block';
    startProgressBar();
    state.playTimer = setInterval(function () { next(); }, PLAY_INTERVAL);
  }

  function stopPlay() {
    state.playing = false;
    if (el.playIcon) {
      el.playIcon.innerHTML = '<polygon points="5 3 19 12 5 21 5 3"/>';
    }
    if (el.progress) el.progress.style.display = 'none';
    clearInterval(state.playTimer);
    stopProgressBar();
  }

  function togglePlay() {
    if (state.playing) stopPlay(); else startPlay();
  }

  function resetPlayTimer() {
    clearInterval(state.playTimer);
    stopProgressBar();
    startProgressBar();
    state.playTimer = setInterval(function () { next(); }, PLAY_INTERVAL);
  }

  function startProgressBar() {
    var bar = el.progressBar;
    bar.style.transition = 'none';
    bar.style.width = '0%';
    requestAnimationFrame(function () {
      bar.style.transition = 'width ' + PLAY_INTERVAL + 'ms linear';
      bar.style.width = '100%';
    });
  }

  function stopProgressBar() {
    el.progressBar.style.transition = 'none';
    el.progressBar.style.width = '0%';
  }

  /* ── Fullscreen ─────────────────────────────────────────────── */
  function toggleFullscreen() {
    if (!document.fullscreenElement) {
      el.overlay.requestFullscreen().catch(function () {});
    } else {
      document.exitFullscreen().catch(function () {});
    }
  }

  /* ── Download ───────────────────────────────────────────────── */
  function download() {
    var items = galleries[state.galleryKey];
    if (!items) return;
    var src = items[state.index].src;
    var a = document.createElement('a');
    a.href = src;
    a.download = src.split('/').pop() || 'image.jpg';
    a.target = '_blank';
    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);
  }

  /* ── Bind all events ────────────────────────────────────────── */
  function bindEvents() {
    // Toolbar + nav buttons
    document.getElementById('mv-close').addEventListener('click', close);
    document.getElementById('mv-prev').addEventListener('click', prev);
    document.getElementById('mv-next').addEventListener('click', next);
    document.getElementById('mv-zoom-in').addEventListener('click', function () { setZoom(state.zoom * 1.5); });
    document.getElementById('mv-zoom-out').addEventListener('click', function () { setZoom(state.zoom / 1.5); });
    document.getElementById('mv-zoom-fit').addEventListener('click', function () { resetZoom(false); });
    document.getElementById('mv-play').addEventListener('click', togglePlay);
    document.getElementById('mv-fullscr').addEventListener('click', toggleFullscreen);
    document.getElementById('mv-download').addEventListener('click', download);

    // Close on backdrop click
    el.backdrop.addEventListener('click', close);

    // Keyboard shortcuts
    document.addEventListener('keydown', function (e) {
      if (!state.open) return;
      switch (e.key) {
        case 'Escape':     close(); break;
        case 'ArrowLeft':  prev(); break;
        case 'ArrowRight': next(); break;
        case ' ':          e.preventDefault(); togglePlay(); break;
        case 'f': case 'F': toggleFullscreen(); break;
        case 'd': case 'D': download(); break;
        case '+': case '=': setZoom(state.zoom * 1.5); break;
        case '-':           setZoom(state.zoom / 1.5); break;
      }
    });

    // Mouse-wheel zoom
    el.frame.addEventListener('wheel', function (e) {
      e.preventDefault();
      var factor = e.deltaY < 0 ? 1.18 : 1 / 1.18;
      setZoom(state.zoom * factor, e.clientX, e.clientY);
    }, { passive: false });

    // Mouse drag to pan
    el.frame.addEventListener('mousedown', function (e) {
      if (state.zoom <= 1) return;
      state.dragging   = true;
      state.dragX      = e.clientX;
      state.dragY      = e.clientY;
      state.panOriginX = state.panX;
      state.panOriginY = state.panY;
      el.img.style.cursor = 'grabbing';
      e.preventDefault();
    });

    document.addEventListener('mousemove', function (e) {
      if (!state.dragging) return;
      state.panX = state.panOriginX + (e.clientX - state.dragX);
      state.panY = state.panOriginY + (e.clientY - state.dragY);
      applyTransform();
    });

    document.addEventListener('mouseup', function () {
      if (state.dragging) {
        state.dragging = false;
        el.img.style.cursor = state.zoom > 1 ? 'grab' : 'default';
      }
    });

    // Touch: swipe to navigate + pinch to zoom + drag when zoomed
    var touchX0 = 0, touchY0 = 0, pinchActive = false;

    document.getElementById('mv-stage').addEventListener('touchstart', function (e) {
      pinchActive = false;
      if (e.touches.length === 2) {
        pinchActive = true;
        state.pinchDist0 = pinchDistance(e);
        state.pinchZoom0 = state.zoom;
        state.dragging   = false;
      } else if (e.touches.length === 1) {
        touchX0 = e.touches[0].clientX;
        touchY0 = e.touches[0].clientY;
        if (state.zoom > 1) {
          state.dragging   = true;
          state.dragX      = touchX0;
          state.dragY      = touchY0;
          state.panOriginX = state.panX;
          state.panOriginY = state.panY;
        }
      }
    }, { passive: true });

    document.getElementById('mv-stage').addEventListener('touchmove', function (e) {
      if (pinchActive && e.touches.length === 2) {
        e.preventDefault();
        var d  = pinchDistance(e);
        var cx = (e.touches[0].clientX + e.touches[1].clientX) / 2;
        var cy = (e.touches[0].clientY + e.touches[1].clientY) / 2;
        setZoom(state.pinchZoom0 * d / state.pinchDist0, cx, cy);
      } else if (state.dragging && state.zoom > 1 && e.touches.length === 1) {
        e.preventDefault();
        state.panX = state.panOriginX + (e.touches[0].clientX - state.dragX);
        state.panY = state.panOriginY + (e.touches[0].clientY - state.dragY);
        applyTransform();
      }
    }, { passive: false });

    document.getElementById('mv-stage').addEventListener('touchend', function (e) {
      state.dragging = false;
      if (pinchActive) { pinchActive = false; return; }
      if (state.zoom > 1 || e.changedTouches.length < 1) return;

      var dx = e.changedTouches[0].clientX - touchX0;
      var dy = e.changedTouches[0].clientY - touchY0;
      if (Math.abs(dx) > 50 && Math.abs(dx) > Math.abs(dy) * 1.4) {
        if (dx < 0) next(); else prev();
      }
    }, { passive: true });

    // Intercept all data-fancybox links site-wide
    document.addEventListener('click', function (e) {
      var link = e.target && e.target.closest ? e.target.closest('a[data-fancybox]') : null;
      if (!link) return;
      e.preventDefault();

      var key = link.getAttribute('data-fancybox') || 'default';
      var links = Array.prototype.slice.call(document.querySelectorAll('a[data-fancybox="' + key + '"]'));
      var idx = links.indexOf(link);

      if (window.mvAllImages && window.mvAllImages.length > 0) {
        galleries[key] = window.mvAllImages.map(function (src) {
          return { src: src, thumb: src, caption: '' };
        });
        open(key, (window.mvPageStartIndex || 0) + (idx >= 0 ? idx : 0));
        return;
      }

      galleries[key] = links.map(function (a) {
        var img = a.querySelector('img');
        return {
          src:     a.getAttribute('href') || a.getAttribute('data-src') || '',
          thumb:   img ? img.src : null,
          caption: a.getAttribute('data-caption') || (img ? img.getAttribute('alt') : '') || '',
        };
      });

      open(key, idx >= 0 ? idx : 0);
    });
  }

  /* ── Helpers ────────────────────────────────────────────────── */
  function pinchDistance(e) {
    var dx = e.touches[0].clientX - e.touches[1].clientX;
    var dy = e.touches[0].clientY - e.touches[1].clientY;
    return Math.sqrt(dx * dx + dy * dy);
  }

  /* ── Bootstrap ──────────────────────────────────────────────── */
  function init() {
    buildUI();
    bindEvents();
  }

  if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', init);
  } else {
    init();
  }

})();
