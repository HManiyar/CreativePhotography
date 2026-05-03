;(function () {
  'use strict';

  /* Only activate on true pointer/mouse devices */
  if (!window.matchMedia('(hover: hover) and (pointer: fine)').matches) return;

  var dot, dotInner, ring, ringInner;
  var mouseX = 0, mouseY = 0;
  var ringX  = 0, ringY  = 0;
  var hidden = true;

  /* ── Build DOM ────────────────────────────────────────────── */
  function build() {
    dot = document.createElement('div');
    dot.id = 'cd-cursor-dot';
    dotInner = document.createElement('div');
    dotInner.className = 'cd-cursor__inner';
    dot.appendChild(dotInner);

    ring = document.createElement('div');
    ring.id = 'cd-cursor-ring';
    ringInner = document.createElement('div');
    ringInner.className = 'cd-cursor__inner';
    ring.appendChild(ringInner);

    document.body.appendChild(dot);
    document.body.appendChild(ring);
  }

  /* ── RAF loop — smooth lag on ring ───────────────────────── */
  function loop() {
    ringX += (mouseX - ringX) * 0.12;
    ringY += (mouseY - ringY) * 0.12;
    ring.style.transform = 'translate(' + ringX + 'px, ' + ringY + 'px)';
    requestAnimationFrame(loop);
  }

  /* ── Visibility ───────────────────────────────────────────── */
  function show() {
    if (!hidden) return;
    hidden = false;
    dot.classList.add('cd-cursor--visible');
    ring.classList.add('cd-cursor--visible');
  }

  function hide() {
    hidden = true;
    dot.classList.remove('cd-cursor--visible');
    ring.classList.remove('cd-cursor--visible');
  }

  /* ── Bind events ──────────────────────────────────────────── */
  function bindEvents() {
    document.addEventListener('mousemove', function (e) {
      show();
      mouseX = e.clientX;
      mouseY = e.clientY;
      dot.style.transform = 'translate(' + e.clientX + 'px, ' + e.clientY + 'px)';
    });

    document.addEventListener('mouseleave', hide);
    document.addEventListener('mouseenter', show);

    /* Interactive hover state */
    document.addEventListener('mouseover', function (e) {
      var interactive = !!e.target.closest(
        'a, button, label, [data-fancybox],' +
        ' .service-card, .album-item, .masonry-item,' +
        ' .mv-btn, .mv-nav, .mv-thumb,' +
        ' input[type="submit"], input[type="button"]'
      );
      ring.classList.toggle('cd-cursor--hover', interactive);
      dot.classList.toggle('cd-cursor--hover',  interactive);
    });

    /* Click pulse */
    document.addEventListener('mousedown', function () {
      ring.classList.add('cd-cursor--click');
      dot.classList.add('cd-cursor--click');
    });
    document.addEventListener('mouseup', function () {
      ring.classList.remove('cd-cursor--click');
      dot.classList.remove('cd-cursor--click');
    });
  }

  /* ── Init ─────────────────────────────────────────────────── */
  function init() {
    build();
    bindEvents();
    loop();
  }

  if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', init);
  } else {
    init();
  }
})();
